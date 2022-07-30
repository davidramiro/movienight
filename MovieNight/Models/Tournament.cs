namespace MovieNight.Models;

public class Tournament
{
    public int Id { get; set; }
    public List<MovieBracket>? MovieBrackets { get; set; }
    public bool Running { get; set; } = false;
    public bool Finished { get; set; } = false;
    public int CurrentRound { get; set; }
    public int HighestBracket { get; set; }
    public DateTime Date { get; set; }
}