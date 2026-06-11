using MaintenanceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asset>(e =>
        {
            e.HasKey(a => a.AssetId);
            e.Property(a => a.Name).IsRequired().HasMaxLength(200);
            e.Property(a => a.Type).IsRequired().HasMaxLength(100);
            e.Property(a => a.Location).IsRequired().HasMaxLength(200);
            e.Property(a => a.Description).HasMaxLength(1000);
            e.Property(a => a.QrCodePayload).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<AuditLog>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.Action).IsRequired().HasMaxLength(200);
            e.Property(a => a.Details).HasMaxLength(1000);
        });
    }
}
