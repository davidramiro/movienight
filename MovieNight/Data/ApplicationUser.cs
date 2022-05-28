using Microsoft.AspNetCore.Identity;

namespace MovieNight.Data;

public class ApplicationUser : IdentityUser
{
    public string firstName { get; set; }
}