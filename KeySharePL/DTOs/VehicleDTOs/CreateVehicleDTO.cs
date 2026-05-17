using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyShareDL.Enums;


namespace KeySharePL.DTOs.VehicleDTOs
{
    public class CreateVehicleDTO
    {
        public required string Model { get; set; }
        public required string Type { get; set; }
        public decimal PricePerDay { get; set; }
        public required string Location { get; set; }
        public string? Description { get; set; }
    }
}
