using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyshareDL.Enums;
using KeyShareDL.Enums;


namespace KeySharePL.DTOs.VehicleDTOs
{
    public class BasicVehicleDTO
    {
        public int VehicleId { get; set; }
        public required string Model { get; set; }
        public required string Type { get; set; }
        public decimal PricePerDay { get; set; }
        public required string Location { get; set; }
        public string? Description { get; set; }
        public VehicleStatus AvailabilityStatus { get; set; }
        public VehicleListingStatus ListingStatus { get; set; }
        public int OwnerId { get; set; }
        public string? OwnerName { get; set; }
        public List<string> ImageUrls { get; set; } = new();
    }
}
