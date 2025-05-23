using Microsoft.EntityFrameworkCore;
using OTPBUILDAPI.Models;

namespace OTPBUILDAPI.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Summoner> Summoners { get; set; }
    public DbSet<PerksStyle> PerksStyles { get; set; }
    public DbSet<StatPerks> StatPerks { get; set; }
    public DbSet<Perks> Perks { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Participant> Participants { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PerksStyle>()
            .HasIndex(p => new
            {
                p.Description,
                p.Style,
                p.StyleSelection1,
                p.StyleSelection2,
                p.StyleSelection3,
                p.StyleSelection4
            })
            .IsUnique()
            .HasDatabaseName("idx_perkstyle_unique");

        modelBuilder.Entity<StatPerks>()
            .HasIndex(s => new { s.Defense, s.Flex, s.Offense })
            .IsUnique()
            .HasDatabaseName("unique_stats");

        modelBuilder.Entity<Perks>()
            .HasIndex(p => new { p.StatPerksId, p.PrimaryStyleId, p.SecondaryStyleId })
            .IsUnique()
            .HasDatabaseName("idx_statPerks_unq");

        modelBuilder.Entity<Participant>()
            .HasKey(p => new { p.GameId, p.SummonerPuuid });

        modelBuilder.Entity<Game>()
            .HasMany(g => g.Participants)
            .WithOne(p => p.Game)
            .HasForeignKey(p => p.GameId);

        modelBuilder.Entity<Participant>()
            .HasOne(p => p.Perks)
            .WithMany()
            .HasForeignKey(p => p.PerksId)  // Utilisez PerksId ici
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Perks>()
            .HasOne(p => p.PrimaryStyle)
            .WithMany()
            .HasForeignKey(p => p.PrimaryStyleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Perks>()
            .HasOne(p => p.SecondaryStyle)
            .WithMany()
            .HasForeignKey(p => p.SecondaryStyleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Participant>()
            .HasOne(p => p.Summoner)
            .WithMany()
            .HasForeignKey(p => p.SummonerPuuid);
    }
}