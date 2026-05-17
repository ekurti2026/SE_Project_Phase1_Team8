using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyshareDL.Enums;
using KeyShareDL.Enums;


namespace KeySharePL.DTOs.VehicleDTOs
{
    public class UpdateVehicleDTO
    {
        public string? Model { get; set; }
        public string? Type { get; set; }
        public decimal? PricePerDay { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public VehicleStatus? AvailabilityStatus { get; set; }
    }
}
