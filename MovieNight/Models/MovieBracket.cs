namespace MovieNight.Models;

public class MovieBracket
{
    public int Id { get; set; }
    public List<MoviePair>? MoviePairs { get; set; }
    public DateTime Date { get; set; }
    public int BracketNumber { get; set; }
}