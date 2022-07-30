using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieNight.Models;

namespace MovieNight.Data;

public class ApplicationDbContext : IdentityDbContext<MovieUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
 
        base.OnModelCreating(builder);
 
        builder.Entity<MovieUser>()
            .Property(e => e.firstName)
            .HasMaxLength(250);
    }
    
    public DbSet<Movie>? Movie { get; set; }
    public DbSet<Vote>? Vote { get; set; }
    public DbSet<Suggestion>? Suggestion { get; set; }
    public DbSet<MovieBracket>? MovieBracket { get; set; }
    public DbSet<MoviePair>? MoviePair { get; set; }
    public DbSet<MovieNight.Models.Tournament>? Tournament { get; set; }
}
