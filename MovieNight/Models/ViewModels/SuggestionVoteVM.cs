namespace MovieNight.Models.ViewModels;

public class SuggestionVoteVM
{
    public Dictionary<Suggestion, List<Vote>> suggestionvote { get; set; }
    public int castVotes { get; set; }
}