using Microsoft.EntityFrameworkCore;
using PrepMeUp.Domain;

namespace PrepMeUp.Infrastructure;

public class PrepMeUpDbContext(DbContextOptions<PrepMeUpDbContext> options) : DbContext(options)
{
    public DbSet<Item> Items => Set<Item>();

    // Called exactly once by EF core to check how the database should be built
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Make this item from Domain into a table
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Name).IsRequired();
        });
    }
}