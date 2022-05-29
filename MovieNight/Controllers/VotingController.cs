using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieNight.Data;
using MovieNight.Models;
using MovieNight.Models.ViewModels;


namespace MovieNight.Controllers
{
    public class VotingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _user;

        public VotingController(ApplicationDbContext context, UserManager<ApplicationUser> user)
        {
            _context = context;
            _user = user;
        }

        // GET: Index
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"].ToString();
            }

            SuggestionVoteVM vm = new SuggestionVoteVM
            {
                suggestionvote = new Dictionary<Suggestion, List<Vote>>()
            };

            var user = await _user.GetUserAsync(HttpContext.User);
            vm.castVotes = await _context.Vote.CountAsync(v => (v.User.Id == user.Id) && (v.Date.Month == DateTime.Today.Month));

            var suggList = await _context.Suggestion
                .Where(s => s.Date.Month == DateTime.Today.Month)
                .Include(s => s.Movie)
                .Include(s => s.User)
                .ToListAsync();
            
            foreach (Suggestion sug in suggList)
            {
                List<Vote> votes = await _context.Vote?.Where(v => v.Suggestion.Id == sug.Id).ToListAsync();
                
                vm.suggestionvote.Add(sug, votes);
            }
            return View(vm);
        }
        
        
        // GET: Voting/Create
        [Authorize]
        public async Task<IActionResult> Vote(int? itemId)
        {
            Suggestion sugg = _context.Suggestion.First(s => s.Id == itemId);
            Vote vote = new Vote
            {
                Date = DateTime.Today
            };

            var user = await _user.GetUserAsync(HttpContext.User);

            bool ownSuggestion = (sugg.User.Id == user.Id);
            bool alreadyVoted = await _context.Vote.AnyAsync(v => (v.Suggestion.Id == itemId && v.User.Id == user.Id));
            bool tooManyVotes = await _context.Vote.CountAsync(v => (v.User.Id == user.Id) && (v.Date.Month == DateTime.Today.Month)) > 2;
            
            if (alreadyVoted)
            {
                TempData["Error"] = "You have already voted for this movie.";
                return RedirectToAction(nameof(Index));
            }
            else if (tooManyVotes)
            {
                TempData["Error"] = "You have already cast all your votes.";
                return RedirectToAction(nameof(Index));
            }
            else if (ownSuggestion)
            {
                TempData["Error"] = "You cannot vote for your own suggestion.";
                return RedirectToAction(nameof(Index));
            }
            
            
            vote.User = user;
            vote.Suggestion = sugg;
            _context.Vote?.Add(vote);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
    }
}
