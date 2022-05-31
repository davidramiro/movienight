using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieNight.Data;
using MovieNight.Models;
using MovieNight.Services;

namespace MovieNight.Controllers
{
    public class SuggestionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly OmdbService _omdb;
        private readonly UserManager<MovieUser> _user;
        private readonly ILogger<SuggestionsController> _logger;

        public SuggestionsController(ApplicationDbContext context, UserManager<MovieUser> user, IConfiguration configuration, ILogger<SuggestionsController> logger)
        {
            _context = context;
            _user = user;
            _omdb = new OmdbService(configuration);
            _logger = logger;
        }

        // GET: Suggestions/Create
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewData.Clear();
            var user = await _user.GetUserAsync(HttpContext.User);
            
            if (_context.Suggestion == null)
            {
                ViewBag.Error = "Error fetching Suggestions.";
                return View();
            }
            
            ViewBag.SuggestionsMade = await _context.Suggestion
                .CountAsync(s => s.User != null && s.User.Id == user.Id && s.Date.Month == DateTime.Today.Month);
            return View();
        }

        // POST: Suggestions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("ImdbId")] ImdbInput imdbInput)
        {
            if (ModelState.IsValid)
            {
                var user = await _user.GetUserAsync(HttpContext.User);
                
                if (_context.Suggestion == null)
                {
                    ViewBag.Error = "Error fetching Suggestions.";
                    ViewBag.SuggestionsMade = "N/A";
                    return View(imdbInput);
                }

                int suggestionsMade = await _context.Suggestion
                    .CountAsync(s => s.User != null && s.User.Id == user.Id
                        && s.Date.Month == DateTime.Today.Month);
                
                if (suggestionsMade > 2)
                {
                    ViewBag.Error = "You cannot make any more suggestions.";
                    ViewBag.SuggestionsMade = suggestionsMade;
                    return View(imdbInput);
                }

                Movie movie;
                
                try
                {
                    movie = FetchOrAssignMovie(imdbInput.ImdbId);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error fetching movie info.";
                    _logger.LogError("Fetching failed: {}", ex.Message);
                    ViewBag.SuggestionsMade = suggestionsMade;
                    return View(imdbInput);
                }
                
                bool alreadySuggested = await _context.Suggestion.AnyAsync(s => s.Movie != null && s.Movie.ImdbId == movie.ImdbId
                 && s.Date.Month == DateTime.Today.Month);

                if (alreadySuggested)
                {
                    ViewBag.Error = "This movie is already suggested for this month.";
                    ViewBag.SuggestionsMade = suggestionsMade;
                    return View(imdbInput);
                }
                
                Suggestion sugg = new Suggestion();
                sugg.User = user;
                sugg.Movie = movie;
                sugg.Date = DateTime.Today;

                _context.Suggestion?.Add(sugg);
                
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Voting");
            }
            return View(imdbInput);
        }
        

        private Movie FetchOrAssignMovie(string imbdId)
        {
            string queryId = _omdb.ConvertIdInput(imbdId);
            
            if ((_context.Movie?.Any(e => e.ImdbId == queryId)).GetValueOrDefault())
            {
                if (_context.Movie == null)
                {
                    ViewBag.Error = "Error fetching Suggestions.";
                    throw new KeyNotFoundException();
                }
                
                return _context.Movie
                    .First(m => m.ImdbId == queryId);
            }
            else
            {
                Movie movie = _omdb.GetMovieInfo(queryId).Result;
                _context.Movie?.Add(movie);

                return movie;
            }
        }
    }
}
