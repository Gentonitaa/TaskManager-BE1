using Microsoft.EntityFrameworkCore;
using TaskManager.DataContext.Models;

public class AppDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Issue> Issues { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Issue>()
                    .HasOne(i => i.Reporter)
                    .WithMany()
                    .HasForeignKey(i => i.ReporterId)
                    .OnDelete(DeleteBehavior.Restrict);


        builder.Entity<Issue>()
                    .HasOne(i => i.Assignee)
                    .WithMany()
                    .HasForeignKey(i => i.AssigneeId)
                    .OnDelete(DeleteBehavior.Restrict);

        //delete coments if the issue is deleted
        builder.Entity<Comment>()
                    .HasOne(c => c.Issue)
                    .WithMany(i => i.Comments)
                    .HasForeignKey(c => c.IssueId)
                    .OnDelete(DeleteBehavior.Cascade);

    }
}

