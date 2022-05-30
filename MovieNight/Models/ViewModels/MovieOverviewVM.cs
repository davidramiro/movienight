namespace MovieNight.Models.ViewModels;

public class MovieOverviewVM
{
    public Dictionary<Suggestion, List<Vote>>? LastMovies { get; set; }
    public bool LastEmpty { get; set; } = true;
    public Dictionary<Suggestion, List<Vote>>? CurrentMovies { get; set; }
    public bool CurrentEmpty { get; set; } = true;
    
}