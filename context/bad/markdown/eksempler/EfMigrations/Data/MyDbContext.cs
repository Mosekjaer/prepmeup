using EfMigrations.Models;
using Microsoft.EntityFrameworkCore;

namespace EfMigrations.Data;

public class MyDbContext: DbContext {

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlServer(
            @"Data Source=127.0.0.1,1433;Database=Education;User ID=SA;Password=Password1;"
            );
    }

    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Instructor> Instructors => Set<Instructor>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Student> Students => Set<Student>();


    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Department>()
                    .HasKey(d => d.DepartmentName);

        modelBuilder.Entity<Department>()
            .HasMany<Instructor>(d => d.Employees)
            .WithOne(i => i.Department)
            .HasForeignKey(i => i.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}