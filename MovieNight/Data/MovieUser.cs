using Microsoft.AspNetCore.Identity;

namespace MovieNight.Data;

public class MovieUser : IdentityUser
{
    public string firstName { get; set; }
}