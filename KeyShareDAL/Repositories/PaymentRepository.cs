using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyShareDAL.Context;
using KeyShareDAL.Interfaces;
using KeyshareDL.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeyShareDAL.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly KeyShareContext _context;

        public PaymentRepository(KeyShareContext context)
        {
            _context = context;
        }

        public Payment? GetPaymentById(int id)
        {
            return _context.Payments
                .Include(p => p.Booking)
                .Include(p => p.User)
                .FirstOrDefault(p => p.PaymentId == id);
        }

        public Payment? GetPaymentByBooking(int bookingId)
        {
            return _context.Payments
                .Include(p => p.Booking)
                .Include(p => p.User)
                .FirstOrDefault(p => p.BookingId == bookingId);
        }

        public List<Payment> GetAllPayments()
        {
            return _context.Payments
                .Include(p => p.Booking)
                .Include(p => p.User)
                .ToList();
        }

        public Payment AddPayment(Payment payment)
        {
            _context.Payments.Add(payment);
            return payment;
        }

        public Payment UpdatePayment(Payment payment)
        {
            _context.Payments.Update(payment);
            return payment;
        }

        public Payment DeletePayment(Payment payment)
        {
            _context.Payments.Remove(payment);
            return payment;
        }
    }
}