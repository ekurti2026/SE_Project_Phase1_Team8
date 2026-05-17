using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyshareDL.Enums;
using KeyShareDL.Enums;


namespace KeySharePL.DTOs.BookingDTOs
{
    public class BasicBookingDTO
    {
        public int BookingId { get; set; }
        public int VehicleId { get; set; }
        public string? VehicleModel { get; set; }
        public int RenterId { get; set; }
        public string? RenterName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public BookingStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
