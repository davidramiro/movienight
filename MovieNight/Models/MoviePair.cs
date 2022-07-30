namespace MovieNight.Models;

public class MoviePair
{
    public int Id { get; set; }
    public Suggestion TopSuggestion { get; set; }
    public Suggestion BottomSuggestion { get; set; }
    public bool winnerFound { get; set; } = false;
    public bool winnerTop { get; set; } = false;
}