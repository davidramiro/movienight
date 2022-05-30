using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MovieNight.Data;

namespace MovieNight.Services;

public class AdditionalUserClaimsPrincipalFactory 
    : UserClaimsPrincipalFactory<MovieUser, IdentityRole>
{
    public AdditionalUserClaimsPrincipalFactory( 
        UserManager<MovieUser> userManager,
        RoleManager<IdentityRole> roleManager, 
        IOptions<IdentityOptions> optionsAccessor) 
        : base(userManager, roleManager, optionsAccessor)
    {}
    
    
    public override async Task<ClaimsPrincipal> CreateAsync(MovieUser user)
    {
        var principal = await base.CreateAsync(user);
        var identity = (ClaimsIdentity)principal.Identity;

        var claims = new List<Claim>();

        claims.Add(new Claim("FirstName", user.firstName));
        identity.AddClaims(claims);
        return principal;
    }
}