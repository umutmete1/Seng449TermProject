using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TermProject.models;

public class AppDbContext : IdentityDbContext<MyUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<UserWatchlist>()
            .HasKey(us => new { us.MyUserId, us.StockId });

        modelBuilder.Entity<UserWatchlist>()
            .HasOne(us => us.MyUser)
            .WithMany(u => u.Watchlist)
            .HasForeignKey(us => us.MyUserId);

        modelBuilder.Entity<UserWatchlist>()
            .HasOne(us => us.Stock)
            .WithMany()
            .HasForeignKey(us => us.StockId);
    }

    public DbSet<Stock> Stocks { get; set; }
    
    public DbSet<UserWatchlist> UserWatchlist { get; set; }
    
}