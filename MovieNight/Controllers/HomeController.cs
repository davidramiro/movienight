using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieNight.Data;
using MovieNight.Models;
using MovieNight.Models.ViewModels;

namespace MovieNight.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {

        MovieOverviewVM vm = new MovieOverviewVM();

        try
        {
            vm.CurrentMovies = ReturnMostVotedSuggestions(0).Result;
            vm.CurrentEmpty = false;
        }
        catch (Exception k)
        {
            _logger.LogInformation("No movies for current month: {}", k.Message);
        }
        
        try
        {
            vm.LastMovies = ReturnMostVotedSuggestions(-1).Result;
            vm.LastEmpty = false;
        }
        catch (Exception k)
        {
            _logger.LogInformation("No movies for last month: {}", k.Message);
        }

        return View(vm);
    }

    private async Task<Dictionary<Suggestion, List<Vote>>> ReturnMostVotedSuggestions(int monthOffset)
    {
        Dictionary<Suggestion, List<Vote>> suggestionDict = new Dictionary<Suggestion, List<Vote>>();
        
        if (_context.Suggestion == null)
        {
            throw new KeyNotFoundException("Suggestions could not be parsed.");
        }
        if (_context.Vote == null)
        {
            throw new KeyNotFoundException("Votes could not be parsed.");
        }
        
        var suggList = await _context.Suggestion
            .Where(s => s.Date.Month == DateTime.Today.Month + monthOffset)
            .Include(s => s.Movie)
            .Include(s => s.User)
            .ToListAsync();

        if (suggList.Count == 0)
        {
            throw new KeyNotFoundException("No movies found for that month.");
        }

        int maxVotes = -1;
        
        foreach (Suggestion sug in suggList)
        {
            List<Vote> votes = await _context.Vote
                .Where(v => v.Suggestion != null && v.Suggestion.Id == sug.Id && v.Date.Month == DateTime.Today.Month + monthOffset)
                .ToListAsync();

            if (votes.Count > maxVotes)
            {
                maxVotes = votes.Count;
            }
            suggestionDict.Add(sug, votes);
        }
        
        foreach(KeyValuePair<Suggestion, List<Vote>> entry in suggestionDict)
        {
            if (entry.Value.Count < maxVotes)
            {
                suggestionDict.Remove(entry.Key);
            }
        }

        return suggestionDict;

    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
