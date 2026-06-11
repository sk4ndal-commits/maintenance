using MaintenanceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
    public DbSet<Technician> Technicians => Set<Technician>();
    public DbSet<AssignmentHistory> AssignmentHistories => Set<AssignmentHistory>();
    public DbSet<ChecklistStep> ChecklistSteps => Set<ChecklistStep>();

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

        modelBuilder.Entity<WorkOrder>(e =>
        {
            e.HasKey(w => w.WorkOrderId);
            e.Property(w => w.Title).IsRequired().HasMaxLength(300);
            e.Property(w => w.Priority).IsRequired().HasMaxLength(50).HasConversion<string>();
            e.Property(w => w.Status).HasConversion<string>();
        });

        modelBuilder.Entity<AuditLog>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.Action).IsRequired().HasMaxLength(200);
            e.Property(a => a.Details).HasMaxLength(1000);
        });

        modelBuilder.Entity<Technician>(e =>
        {
            e.HasKey(t => t.TechnicianId);
            e.Property(t => t.Name).IsRequired().HasMaxLength(200);
            e.Property(t => t.Email).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<AssignmentHistory>(e =>
        {
            e.HasKey(h => h.Id);
            e.Property(h => h.AssignedBy).IsRequired().HasMaxLength(200);
            e.Property(h => h.TechnicianName).HasMaxLength(200);
        });

        modelBuilder.Entity<ChecklistStep>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.Label).IsRequired().HasMaxLength(500);
        });
    }
}
