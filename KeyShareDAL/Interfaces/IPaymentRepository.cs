using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyshareDL.Entities;
using KeyShareDL.Entities;

namespace KeyShareDAL.Interfaces
{
    public interface IPaymentRepository
    {
        Payment? GetPaymentById(int id);
        Payment? GetPaymentByBooking(int bookingId);
        List<Payment> GetAllPayments();
        Payment AddPayment(Payment payment);
        Payment UpdatePayment(Payment payment);
        Payment DeletePayment(Payment payment);
    }
}
