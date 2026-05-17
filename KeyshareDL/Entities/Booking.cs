using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyshareDL.Enums;
using KeyShareDL.Entities;

namespace KeyshareDL.Entities
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // FK
        public int RenterId { get; set; }
        public User Renter { get; set; } = null!;

        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;

        // Navigation
        public Payment? Payment { get; set; }
        public Review? Review { get; set; }
    }
}
