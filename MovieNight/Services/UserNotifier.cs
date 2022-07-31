using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MovieNight.Data;
using MovieNight.Models;

namespace MovieNight.Services;

public class UserNotifier : IUserNotifier
{
    private readonly ILogger<UserNotifier> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IEmailSender _emailSender;

    public UserNotifier(ILogger<UserNotifier> logger, ApplicationDbContext context, IEmailSender emailSender)
    {
        _logger = logger;
        _context = context;
        _emailSender = emailSender;
    }
    
    
    public void NotifyUsers()
    {
        var winner = GetLastMonthsWinner().First();
        
        var users = _context.Users.ToList();
        foreach (var user in users)
        {
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendFormat("Hey {0},<br><br>", user.firstName);
            mailBody.AppendFormat("thank you for participating in this month's movie vote. We got a winner, and it's {0} with {1} votes!<br><br>", winner.Key.Movie.Title, winner.Value.Count);
            mailBody.AppendFormat("Plot: {0}<br><br>", winner.Key.Movie.Plot);
            mailBody.AppendFormat("<a href=\"https://imdb.com/title/{0}/\">Movie info on IMDb</a><br><br>", winner.Key.Movie.ImdbId);
            mailBody.Append("Suggested by: ");

            int count = 0;
            foreach (Vote vote in winner.Value)
            {
                count++;
                mailBody.AppendFormat("{0}", vote.User.firstName);
                if (winner.Value.Count > 1 && count != winner.Value.Count)
                {
                    mailBody.Append(", ");
                }
            }
            
            mailBody.Append(".<br><br>");
            mailBody.Append("Have fun!");

            string subject = "MovieNight " + 
                CultureInfo.CurrentCulture.DateTimeFormat
                .GetMonthName((DateTime.Today.Month))
                +"'s Winner";
            
            _emailSender.SendEmailAsync(user.Email, subject, mailBody.ToString());
            _logger.LogInformation("Sent email to {}", user.Email);
        }
    }

    private Dictionary<Suggestion, List<Vote>> GetLastMonthsWinner()
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
        
        var suggList = _context.Suggestion
            .Where(s => s.Date.Month == DateTime.Today.Month - 1)
            .Include(s => s.Movie)
            .Include(s => s.User)
            .ToList();

        if (suggList.Count == 0)
        {
            throw new KeyNotFoundException("No movies found for that month.");
        }

        int maxVotes = -1;
        
        foreach (Suggestion sug in suggList)
        {
            List<Vote> votes = _context.Vote
                .Where(v => v.Suggestion != null && v.Suggestion.Id == sug.Id && v.Date.Month == DateTime.Today.Month - 1)
                .ToList();

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
}