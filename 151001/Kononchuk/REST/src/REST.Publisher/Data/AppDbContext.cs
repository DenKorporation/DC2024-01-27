using Microsoft.EntityFrameworkCore;
using REST.Publisher.Models.Entities;

namespace REST.Publisher.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Editor> Editors { get; set; } = null!;
    public DbSet<Issue> Issues { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}