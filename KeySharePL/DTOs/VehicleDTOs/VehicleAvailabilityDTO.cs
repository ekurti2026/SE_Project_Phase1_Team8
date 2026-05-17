using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyShareDL.Enums;


namespace KeySharePL.DTOs.VehicleDTOs
{
    public class VehicleAvailabilityDTO
    {
        public int VehicleId { get; set; }
        public bool IsAvailable { get; set; }
        public DateOnly? AvailableFrom { get; set; }
        public DateOnly? AvailableTo { get; set; }
        public List<string> BlockedDates { get; set; } = new();
    }
}
