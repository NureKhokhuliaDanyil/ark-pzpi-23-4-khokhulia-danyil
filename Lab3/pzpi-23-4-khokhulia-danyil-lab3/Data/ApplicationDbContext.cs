using Microsoft.EntityFrameworkCore;
using Washing.Entities;

namespace Washing.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Laundry> Laundries { get; set; }
    public DbSet<WashingMachine> WashingMachines { get; set; }
    public DbSet<WashMode> WashModes { get; set; }
    public DbSet<WashingSession> WashingSessions { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<MaintenanceLog> MaintenanceLogs { get; set; }
    public DbSet<PromoCode> PromoCodes { get; set; }
    public DbSet<TimePricingCondition> TimePricingConditions { get; set; }
    public DbSet<LoadPricingCondition> LoadPricingConditions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Balance).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<WashMode>(entity =>
        {
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<WashingSession>(entity =>
        {
            entity.Property(e => e.ActualPrice).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Laundry>()
            .HasOne(l => l.Owner)
            .WithMany(u => u.OwnedLaundries)
            .HasForeignKey(l => l.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WashingMachine>()
            .HasOne(m => m.Laundry)
            .WithMany(l => l.WashingMachines)
            .HasForeignKey(m => m.LaundryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WashMode>()
            .HasOne(m => m.Laundry)
            .WithMany(l => l.WashModes)
            .HasForeignKey(m => m.LaundryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WashingSession>()
            .HasOne(s => s.User)
            .WithMany(u => u.WashingSessions)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WashingSession>()
            .HasOne(s => s.Machine)
            .WithMany(m => m.WashingSessions)
            .HasForeignKey(s => s.MachineId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WashingSession>()
            .HasOne(s => s.Mode)
            .WithMany(m => m.WashingSessions)
            .HasForeignKey(s => s.ModeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Laundry)
            .WithMany(l => l.Reviews)
            .HasForeignKey(r => r.LaundryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MaintenanceLog>()
            .HasOne(m => m.Machine)
            .WithMany(w => w.MaintenanceLogs)
            .HasForeignKey(m => m.MachineId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MaintenanceLog>(entity =>
        {
            entity.Property(e => e.Cost).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<PromoCode>(entity =>
        {
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18,2)");
            entity.HasIndex(e => e.Code).IsUnique();
        });

        modelBuilder.Entity<TimePricingCondition>(entity =>
        {
            entity.Property(e => e.Multiplier).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<LoadPricingCondition>(entity =>
        {
            entity.Property(e => e.LoadThreshold).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Multiplier).HasColumnType("decimal(18,2)");
        });
    }
}
