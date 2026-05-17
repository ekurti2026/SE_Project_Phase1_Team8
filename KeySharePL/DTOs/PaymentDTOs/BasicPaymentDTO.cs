using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyshareDL.Enums;
using KeyShareDL.Enums;


namespace KeySharePL.DTOs.PaymentDTOs
{
    public class BasicPaymentDTO
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime PaymentDate { get; set; }
        public int BookingId { get; set; }
        public int UserId { get; set; }
    }
}
