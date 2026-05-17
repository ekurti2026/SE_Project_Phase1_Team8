using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyShareDL.Entities;

namespace KeyshareDL.Entities
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        public int Rating { get; set; }
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // FK
        public int ReviewerId { get; set; }
        public User Reviewer { get; set; } = null!;

        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;

        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;
    }
}
