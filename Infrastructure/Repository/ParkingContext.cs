using Microsoft.EntityFrameworkCore;
using ParkingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSystem.Infrastructure.Repository
{
    public partial class ParkingContext : DbContext
    {
        public ParkingContext()
        {
        }

        public ParkingContext(DbContextOptions<ParkingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ParkingOrder> ParkingOrders { get; set; }
        public virtual DbSet<Slot> Slots { get; set; }
        public virtual DbSet<SlotItem> SlotItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=localhost,1433;Database=AisSlots;User Id=sa; Password=aisgaming123#");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ParkingOrder>(entity =>
            {
                entity.ToTable("ParkingOrder");

                entity.HasIndex(e => e.LicensePlate, "ix_parking_order_licenseplate");

                entity.Property(e => e.CheckIn).HasColumnType("datetime");

                entity.Property(e => e.CheckOut).HasColumnType("datetime");

                entity.Property(e => e.Colour)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasColumnType("datetime");

                entity.Property(e => e.LicensePlate)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ParkingFee).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ParkingFeePerHour).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Vehicle)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Slot>(entity =>
            {
                entity.ToTable("Slot");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<SlotItem>(entity =>
            {
                entity.ToTable("SlotItem");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
