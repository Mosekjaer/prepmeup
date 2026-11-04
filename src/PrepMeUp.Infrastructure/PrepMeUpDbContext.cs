using Microsoft.EntityFrameworkCore;
using PrepMeUp.Domain;

namespace PrepMeUp.Infrastructure;

public class PrepMeUpDbContext(DbContextOptions<PrepMeUpDbContext> options) : DbContext(options)
{
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();

    // Called exactly once by EF core to check how the database should be built
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Make this item from Domain into a table
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Name).IsRequired();
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(customer => customer.Id);
            entity.Property(customer => customer.KeycloakId).IsRequired().HasMaxLength(255);
            entity.HasIndex(customer => customer.KeycloakId).IsUnique();
            entity.Property(customer => customer.Name).IsRequired().HasMaxLength(Customer.NameMaxLength);
            entity.Property(customer => customer.Address).IsRequired().HasMaxLength(Customer.AddressMaxLength);
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(subscription => subscription.Id);
            entity.HasOne<Customer>().WithOne().HasForeignKey<Subscription>(subscription => subscription.CustomerId);
            entity.HasMany(subscription => subscription.Orders).WithOne().HasForeignKey(order => order.SubscriptionId);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");
            entity.HasKey(order => order.Id);
        });
    }
}