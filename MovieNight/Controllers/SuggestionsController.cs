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
        private readonly UserManager<ApplicationUser> _user;

        public SuggestionsController(ApplicationDbContext context, UserManager<ApplicationUser> user, IConfiguration configuration)
        {
            _context = context;
            _user = user;
            _omdb = new OmdbService(configuration);
        }

        // GET: Suggestions/Create
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewBag.Error = null;
            var user = await _user.GetUserAsync(HttpContext.User);
            ViewBag.SuggestionsMade = await _context.Suggestion.CountAsync(s => s.User.Id == user.Id
                && s.Date.Month == DateTime.Today.Month);
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

                int suggestionsMade = await _context.Suggestion.CountAsync(s => s.User.Id == user.Id
                    && s.Date.Month == DateTime.Today.Month);
                
                if (suggestionsMade > 2)
                {
                    ViewBag.Error = "You cannot make any more suggestions.";
                    ViewBag.SuggestionsMade = suggestionsMade;
                    return View(imdbInput);
                }

                Movie movie = null;
                
                try
                {
                    movie = FetchOrAssignMovie(imdbInput.ImdbId);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error fetching movie info.";
                    ViewBag.SuggestionsMade = suggestionsMade;
                    return View(imdbInput);
                }
                
                bool alreadySuggested = await _context.Suggestion.AnyAsync(s => s.Movie.ImdbId == movie.ImdbId
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
                return _context.Movie?
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
