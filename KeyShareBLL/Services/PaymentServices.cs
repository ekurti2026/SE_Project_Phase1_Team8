using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KeyShareBLL.Interfaces;
using KeyShareDAL.UnitOfWork;
using KeyshareDL.Entities;
using KeyshareDL.Enums;
using KeyShareDL.Entities;
using KeyShareDL.Enums;
using KeySharePL.DTOs.PaymentDTOs;

namespace KeyShareBLL.Services
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationServices _notificationServices;

        public PaymentServices(IUnitOfWork unitOfWork, IMapper mapper, INotificationServices notificationServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationServices = notificationServices;
        }

        public BasicPaymentDTO GetPayment(int id)
        {
            var payment = _unitOfWork.Payments.GetPaymentById(id);
            if (payment == null)
                throw new KeyNotFoundException($"Payment with ID {id} not found.");

            return _mapper.Map<BasicPaymentDTO>(payment);
        }

        public BasicPaymentDTO GetPaymentByBooking(int bookingId)
        {
            var payment = _unitOfWork.Payments.GetPaymentByBooking(bookingId);
            if (payment == null)
                throw new KeyNotFoundException($"No payment found for booking ID {bookingId}.");

            return _mapper.Map<BasicPaymentDTO>(payment);
        }

        public BasicPaymentDTO SimulatePayment(SimulatePaymentDTO dto)
        {
            if (dto.Amount <= 0)
                throw new ArgumentException("Payment amount must be greater than zero.");

            var booking = _unitOfWork.Bookings.GetBookingById(dto.BookingId);
            if (booking == null)
                throw new KeyNotFoundException($"Booking with ID {dto.BookingId} not found.");

            if (booking.Status != BookingStatus.Approved)
                throw new InvalidOperationException("Payment can only be simulated for approved bookings.");

            var existingPayment = _unitOfWork.Payments.GetPaymentByBooking(dto.BookingId);
            if (existingPayment != null)
                throw new InvalidOperationException("A payment already exists for this booking.");

            var payment = new Payment
            {
                BookingId = dto.BookingId,
                UserId = booking.RenterId,
                Amount = dto.Amount,
                Status = PaymentStatus.Pending,
                PaymentDate = DateTime.UtcNow
            };

            _unitOfWork.Payments.AddPayment(payment);

            // Move booking to PaymentPending
            booking.Status = BookingStatus.PaymentPending;
            _unitOfWork.Bookings.UpdateBooking(booking);

            _unitOfWork.Save();

            return _mapper.Map<BasicPaymentDTO>(payment);
        }

        public BasicPaymentDTO ConfirmPayment(int paymentId)
        {
            var payment = _unitOfWork.Payments.GetPaymentById(paymentId);
            if (payment == null)
                throw new KeyNotFoundException($"Payment with ID {paymentId} not found.");

            if (payment.Status != PaymentStatus.Pending)
                throw new InvalidOperationException($"Cannot confirm a payment that is already '{payment.Status}'.");

            payment.Status = PaymentStatus.Successful;
            _unitOfWork.Payments.UpdatePayment(payment);

            // Confirm the booking
            var booking = payment.Booking;
            booking.Status = BookingStatus.Confirmed;
            _unitOfWork.Bookings.UpdateBooking(booking);

            _unitOfWork.Save();

            _notificationServices.SendNotification(
                booking.RenterId,
                $"Payment confirmed for your booking of {booking.Vehicle?.Model}. Your rental is confirmed!",
                "PaymentConfirmed");

            return _mapper.Map<BasicPaymentDTO>(payment);
        }

        public BasicPaymentDTO FailPayment(int paymentId)
        {
            var payment = _unitOfWork.Payments.GetPaymentById(paymentId);
            if (payment == null)
                throw new KeyNotFoundException($"Payment with ID {paymentId} not found.");

            if (payment.Status != PaymentStatus.Pending)
                throw new InvalidOperationException($"Cannot fail a payment that is already '{payment.Status}'.");

            payment.Status = PaymentStatus.Failed;
            _unitOfWork.Payments.UpdatePayment(payment);

            // Mark booking as PaymentFailed
            var booking = payment.Booking;
            booking.Status = BookingStatus.PaymentFailed;
            _unitOfWork.Bookings.UpdateBooking(booking);

            _unitOfWork.Save();

            _notificationServices.SendNotification(
                booking.RenterId,
                $"Payment failed for your booking of {booking.Vehicle?.Model}. Please try again.",
                "PaymentFailed");

            return _mapper.Map<BasicPaymentDTO>(payment);
        }
    }
}
