using Basic_Auth.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Basic_Auth.Model;
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
