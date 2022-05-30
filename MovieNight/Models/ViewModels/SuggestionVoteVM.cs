namespace MovieNight.Models.ViewModels;

public class SuggestionVoteVM
{
    public Dictionary<Suggestion, List<Vote>>? SuggestionWithVotes { get; set; }
    public int CastVotes { get; set; }
}