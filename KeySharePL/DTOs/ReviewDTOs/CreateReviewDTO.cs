using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeySharePL.DTOs.ReviewDTOs
{
    public class CreateReviewDTO
    {
        public int BookingId { get; set; }
        public int VehicleId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
