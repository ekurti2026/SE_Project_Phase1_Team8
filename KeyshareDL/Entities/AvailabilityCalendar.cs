using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyshareDL.Entities
{
    public class AvailabilityCalendar
    {
        [Key]
        public int CalendarId { get; set; }

        public DateOnly AvailableFrom { get; set; }
        public DateOnly AvailableTo { get; set; }

        /// <summary>Comma-separated blocked dates e.g. "2025-06-01,2025-06-02"</summary>
        public string? BlockedDates { get; set; }

        // FK (one-to-one)
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
    }
}
