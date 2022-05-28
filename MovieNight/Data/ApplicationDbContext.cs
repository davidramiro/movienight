using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieNight.Models;

namespace MovieNight.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
 
        base.OnModelCreating(modelBuilder);
 
        modelBuilder.Entity<ApplicationUser>()
            .Property(e => e.firstName)
            .HasMaxLength(250);
    }
    
    public DbSet<Movie>? Movie { get; set; }
    public DbSet<Vote>? Vote { get; set; }
    public DbSet<Suggestion>? Suggestion { get; set; }
}
