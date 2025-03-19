using Basic_Auth.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Basic_Auth.Model;
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Blog_Model> Blogs { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();  // Convert Enum to String
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<User>()
            .Property(u => u.CreatedAt)
            .ValueGeneratedOnAdd(); // Set only on insert
        
        modelBuilder.Entity<Blog_Model>()
            .HasOne(b => b.User)
            .WithMany(u => u.Blogs)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        base.OnModelCreating(modelBuilder);
    }
}
