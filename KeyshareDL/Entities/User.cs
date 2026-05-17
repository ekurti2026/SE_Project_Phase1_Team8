using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyshareDL.Entities;
using KeyShareDL.Enums;

namespace KeyShareDL.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        public string? PhoneNumber { get; set; }

        public UserRole Role { get; set; }

        public string Status { get; set; } = "Active";

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}