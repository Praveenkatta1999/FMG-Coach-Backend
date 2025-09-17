using Microsoft.EntityFrameworkCore;

public class GameDbContext : DbContext
{
    public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }
    public DbSet<Coach> Coaches { get; set; }

    public DbSet<Team> Teams { get; set; }

    public DbSet<Game> Games { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
