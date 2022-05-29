namespace MovieNight.Models.ViewModels;

public class MovieOverviewVM
{
    public Suggestion LastSuggestion { get; set; }
    public int LastVotes { get; set; }
    public bool LastEmpty { get; set; } = true;
    
    public Suggestion CurrentSuggestion { get; set; }
    public int CurrentVotes { get; set; }
    public bool CurrentEmpty { get; set; } = true;
}