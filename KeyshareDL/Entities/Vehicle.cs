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
    public class Vehicle
    {
        [Key]
        public int VehicleId { get; set; }

        public required string Model { get; set; }
        public required string Type { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PricePerDay { get; set; }

        public required string Location { get; set; }
        public string? Description { get; set; }

        public VehicleStatus AvailabilityStatus { get; set; } = VehicleStatus.Available;
        public VehicleListingStatus ListingStatus { get; set; } = VehicleListingStatus.Draft;

        // FK
        public int OwnerId { get; set; }
        public User Owner { get; set; } = null!;

        // Navigation
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<VehicleImage> Images { get; set; } = new List<VehicleImage>();
        public AvailabilityCalendar? AvailabilityCalendar { get; set; }
    }
}
