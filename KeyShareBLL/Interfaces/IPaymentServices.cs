using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeySharePL.DTOs.PaymentDTOs;

namespace KeyShareBLL.Interfaces
{
    public interface IPaymentServices
    {
        BasicPaymentDTO GetPayment(int id);
        BasicPaymentDTO GetPaymentByBooking(int bookingId);
        BasicPaymentDTO SimulatePayment(SimulatePaymentDTO dto);
        BasicPaymentDTO ConfirmPayment(int paymentId);
        BasicPaymentDTO FailPayment(int paymentId);
    }
}