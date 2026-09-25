using BattleGameFunction.Models;
using Microsoft.EntityFrameworkCore;

namespace BattleGameFunction.Data;

public class BattleGameDbContext : DbContext
{
    public BattleGameDbContext(
        DbContextOptions<BattleGameDbContext> options)
        : base(options)
    {
    }

    public DbSet<Player> Players => Set<Player>();
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<PlayerAsset> PlayerAssets => Set<PlayerAsset>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>().ToTable("Player");
        modelBuilder.Entity<Asset>().ToTable("Asset");
        modelBuilder.Entity<PlayerAsset>().ToTable("PlayerAsset");

        modelBuilder.Entity<PlayerAsset>()
            .HasKey(x => new { x.PlayerId, x.AssetId });

        modelBuilder.Entity<PlayerAsset>()
            .HasOne(x => x.Player)
            .WithMany(x => x.PlayerAssets)
            .HasForeignKey(x => x.PlayerId);

        modelBuilder.Entity<PlayerAsset>()
            .HasOne(x => x.Asset)
            .WithMany(x => x.PlayerAssets)
            .HasForeignKey(x => x.AssetId);
    }
}