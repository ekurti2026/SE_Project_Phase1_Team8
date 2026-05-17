using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using KeyshareDL.Entities;
using KeyshareDL.Enums;
using KeyShareDL.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeyShareDAL.Context
{
    public class KeyShareContext : DbContext
    {
        public KeyShareContext(DbContextOptions<KeyShareContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        public DbSet<VehicleImage> VehicleImages { get; set; } = null!;
        public DbSet<AvailabilityCalendar> AvailabilityCalendars { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User email must be unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Vehicle -> Owner (many vehicles per user)
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Owner)
                .WithMany(u => u.Vehicles)
                .HasForeignKey(v => v.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Booking -> Renter
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Renter)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.RenterId)
                .OnDelete(DeleteBehavior.Restrict);

            // Booking -> Vehicle
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Vehicle)
                .WithMany(v => v.Bookings)
                .HasForeignKey(b => b.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment -> Booking (one-to-one)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId)
                .IsRequired(false);

            // Payment -> User
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Review -> Booking (one-to-one)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Booking)
                .WithOne(b => b.Review)
                .HasForeignKey<Review>(r => r.BookingId);

            // Review -> Reviewer
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Reviewer)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Review -> Vehicle
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Vehicle)
                .WithMany(v => v.Reviews)
                .HasForeignKey(r => r.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Message -> Sender
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Message -> Receiver
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Notification -> User
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // AvailabilityCalendar -> Vehicle (one-to-one)
            modelBuilder.Entity<AvailabilityCalendar>()
                .HasOne(a => a.Vehicle)
                .WithOne(v => v.AvailabilityCalendar)
                .HasForeignKey<AvailabilityCalendar>(a => a.VehicleId);

            // VehicleImage -> Vehicle
            modelBuilder.Entity<VehicleImage>()
                .HasOne(i => i.Vehicle)
                .WithMany(v => v.Images)
                .HasForeignKey(i => i.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
