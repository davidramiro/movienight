using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
            var current = ReturnMostVotedSuggestion(0).Result;
            vm.CurrentSuggestion = current.sugg;
            vm.CurrentVotes = current.count;
            vm.CurrentEmpty = false;
        }
        catch (Exception k)
        {
            _logger.LogInformation("No movies for current month");
            return View(vm);
        }
        
        
        
        try
        {
            var last = ReturnMostVotedSuggestion(-1).Result;
            vm.LastSuggestion = last.sugg;
            vm.LastVotes = last.count;
            vm.LastEmpty = false;
        }
        catch (Exception k)
        {
            _logger.LogInformation("No movies for last month");
            return View(vm);
        }
        
        

        return View(vm);
    }

    private async Task<(Suggestion sugg, int count)> ReturnMostVotedSuggestion(int monthOffset)
    {
        Dictionary<Suggestion, int> suggestionDict = new Dictionary<Suggestion, int>();
        
        var suggList = await _context.Suggestion
            .Where(s => s.Date.Month == DateTime.Today.Month + monthOffset)
            .Include(s => s.Movie)
            .Include(s => s.User)
            .ToListAsync();

        if (suggList.Count == 0)
        {
            throw new KeyNotFoundException("No movies found for that month.");
        }
            
        foreach (Suggestion sug in suggList)
        {
            List<Vote> votes = await _context.Vote?
                .Where(v => v.Suggestion.Id == sug.Id && v.Date.Month + monthOffset == DateTime.Today.Month + monthOffset)
                .ToListAsync();
                
            suggestionDict.Add(sug, votes.Count);
        }
        
        int highestVoteCount = suggestionDict.Values.Max();
        Suggestion mostVotedSuggestion = suggestionDict
            .Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

        return (mostVotedSuggestion, highestVoteCount);
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
