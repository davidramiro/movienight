using MovieNight.Data;

namespace MovieNight.Models;

public class Vote
{
    public int Id { get; set; }
    public ApplicationUser? User { get; set; }
    public Suggestion? Suggestion { get; set; }
    public DateTime Date { get; set; }
}