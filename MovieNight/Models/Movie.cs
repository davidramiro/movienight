using MovieNight.Data;

namespace MovieNight.Models;

public class Movie
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public int ReleaseYear { get; set; }
    public string? Runtime { get; set; }
    public string? Genre { get; set; }
    public string? Plot { get; set; }
    public string? PosterUrl { get; set; }
    public int MetaScore { get; set; }
    public float ImdbRating { get; set; }
    public string? ImdbId { get; set; }
}