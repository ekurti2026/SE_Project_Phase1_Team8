using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeySharePL.DTOs.PaymentDTOs
{
    public class SimulatePaymentDTO
    {
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
    }
}
