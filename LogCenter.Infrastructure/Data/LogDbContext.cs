using LogCenter.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LogCenter.Infrastructure.Data
{
    public class LogDbContext : DbContext
    {
        public LogDbContext(DbContextOptions<LogDbContext> options) : base(options)
        {
        }

        public DbSet<LogEntry> LogEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure LogEntry entity
            modelBuilder.Entity<LogEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Timestamp).IsRequired();
                entity.Property(e => e.Level).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.Application).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Environment).HasMaxLength(50);
                entity.Property(e => e.MachineName).HasMaxLength(100);
                entity.Property(e => e.Category).HasMaxLength(100);
                
                // Create indexes for common query patterns
                entity.HasIndex(e => e.Timestamp);
                entity.HasIndex(e => e.Level);
                entity.HasIndex(e => e.Application);
            });
        }
    }
} 