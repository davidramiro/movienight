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
        private readonly UserManager<MovieUser> _user;

        public VotingController(ApplicationDbContext context, UserManager<MovieUser> user)
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
            if (TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Success"].ToString();
            }

            SuggestionVoteVM vm = new SuggestionVoteVM
            {
                SuggestionWithVotes = new Dictionary<Suggestion, List<Vote>>()
            };

            var user = await _user.GetUserAsync(HttpContext.User);

            if (_context.Vote == null)
            {
                ViewBag.Error = "Error fetching Votes.";
                return View(vm);
            }
            if (_context.Suggestion == null)
            {
                ViewBag.Error = "Error fetching Suggestions.";
                return View(vm);
            }
            
            vm.OwnVotes = await _context.Vote
                .Where(v => v.User != null && v.User.Id == user.Id && v.Date.Month == DateTime.Today.Month)
                .Include(v => v.Suggestion)
                .ToListAsync();

            var suggList = await _context.Suggestion
                .Include(s => s.Movie)
                .Include(s => s.User)
                .Where(s => s.Date.Month == DateTime.Today.Month)
                .ToListAsync();
            
            foreach (Suggestion sug in suggList)
            {
                List<Vote> votes = await _context.Vote
                    .Where(v => v.Suggestion != null && v.Suggestion.Id == sug.Id)
                    .ToListAsync();
                
                vm.SuggestionWithVotes.Add(sug, votes);
            }
            return View(vm);
        }
        
        
        // GET: Voting/Create
        [Authorize]
        public async Task<IActionResult> Vote(int? itemId)
        {
            if (_context.Vote == null)
            {
                TempData["Error"] = "Error fetching Votes.";
                return RedirectToAction(nameof(Index));
            }
            if (_context.Suggestion == null)
            {
                TempData["Error"] = "Error fetching Suggestions.";
                return RedirectToAction(nameof(Index));
            }
            
            Suggestion sugg = _context.Suggestion.Include(s => s.User).First(s => s.Id == itemId);
            Vote vote = new Vote
            {
                Date = DateTime.Today
            };

            var user = await _user.GetUserAsync(HttpContext.User);

            bool ownSuggestion = (sugg.User != null && sugg.User.Id == user.Id);
            bool alreadyVoted = await _context.Vote
                .AnyAsync(v => v.Suggestion != null && v.Suggestion.Id == itemId && v.User != null && v.User.Id == user.Id);
            bool tooManyVotes = await _context.Vote
                .CountAsync(v => v.User != null && v.User.Id == user.Id && v.Date.Month == DateTime.Today.Month) > 2;
            
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
            
            TempData["Success"] = "Vote successfully submitted.";
            return RedirectToAction(nameof(Index));
        }
        
        // GET: Voting/Create
        [Authorize]
        public async Task<IActionResult> RemoveVote(int? voteId)
        {
            var user = await _user.GetUserAsync(HttpContext.User);
            
            if (_context.Vote == null)
            {
                TempData["Error"] = "Error fetching Votes.";
                return RedirectToAction(nameof(Index));
            }
            
            var vote = await _context.Vote.Include(v => v.User).FirstAsync(v => v.Id == voteId);

            bool ownVote = vote.User != null && user.Id == vote.User.Id;
            
            if (!ownVote)
            {
                TempData["Error"] = "You can only undo your own votes.";
                return RedirectToAction(nameof(Index));
            }
            
            _context.Vote?.Remove(vote);
            await _context.SaveChangesAsync();
            
            TempData["Success"] = "Vote successfully removed.";
            return RedirectToAction(nameof(Index));
        }
    }
}
