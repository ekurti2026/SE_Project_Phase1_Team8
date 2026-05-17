using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyShareDL.Enums;


namespace KeySharePL.DTOs.VehicleDTOs
{
    public class SearchVehicleDTO
    {
        public string? Location { get; set; }
        public string? Type { get; set; }
        public decimal? MaxPricePerDay { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
