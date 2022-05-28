using MovieNight.Data;

namespace MovieNight.Models;

public class Suggestion
{
    public int Id { get; set; }
    public ApplicationUser? User { get; set; }
    public Movie? Movie { get; set; }
    public DateTime Date { get; set; }
    
}