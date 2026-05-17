using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeySharePL.DTOs.ReviewDTOs
{
    public class BasicReviewDTO
    {
        public int ReviewId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public int ReviewerId { get; set; }
        public string? ReviewerName { get; set; }
        public int VehicleId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
