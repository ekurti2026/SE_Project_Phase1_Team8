using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyshareDL.Entities
{
    public class VehicleImage
    {
        [Key]
        public int ImageId { get; set; }

        public required string ImageUrl { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // FK
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
    }
}
