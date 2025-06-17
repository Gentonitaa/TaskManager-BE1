using Microsoft.EntityFrameworkCore;
using TaskManager.DataContext.Models;

public class AppDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Issue> Issues { get; set; }
    public DbSet<Comment> Comments { get; set; }


}

