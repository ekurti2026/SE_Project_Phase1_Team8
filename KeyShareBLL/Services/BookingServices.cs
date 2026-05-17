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
using KeySharePL.DTOs.BookingDTOs;

namespace KeyShareBLL.Services
{
    public class BookingServices : IBookingServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationServices _notificationServices;

        public BookingServices(IUnitOfWork unitOfWork, IMapper mapper, INotificationServices notificationServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationServices = notificationServices;
        }

        public BasicBookingDTO GetBooking(int id)
        {
            var booking = _unitOfWork.Bookings.GetBookingById(id);
            if (booking == null)
                throw new KeyNotFoundException($"Booking with ID {id} not found.");

            return _mapper.Map<BasicBookingDTO>(booking);
        }

        public List<BasicBookingDTO> GetBookingsByRenter(int renterId)
        {
            var user = _unitOfWork.Users.GetUserById(renterId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {renterId} not found.");

            var bookings = _unitOfWork.Bookings.GetBookingsByRenter(renterId);
            return _mapper.Map<List<BasicBookingDTO>>(bookings);
        }

        public List<BasicBookingDTO> GetPendingBookingsForOwner(int ownerId)
        {
            var owner = _unitOfWork.Users.GetUserById(ownerId);
            if (owner == null)
                throw new KeyNotFoundException($"Owner with ID {ownerId} not found.");

            var bookings = _unitOfWork.Bookings.GetPendingBookingsForOwner(ownerId);
            return _mapper.Map<List<BasicBookingDTO>>(bookings);
        }

        public BasicBookingDTO CreateBooking(int renterId, CreateBookingDTO dto)
        {
            if (dto.StartDate >= dto.EndDate)
                throw new ArgumentException("Start date must be before end date.");

            if (dto.StartDate < DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException("Start date cannot be in the past.");

            var renter = _unitOfWork.Users.GetUserById(renterId);
            if (renter == null)
                throw new KeyNotFoundException($"Renter with ID {renterId} not found.");

            var vehicle = _unitOfWork.Vehicles.GetVehicleById(dto.VehicleId);
            if (vehicle == null)
                throw new KeyNotFoundException($"Vehicle with ID {dto.VehicleId} not found.");

            if (vehicle.ListingStatus != VehicleListingStatus.Active)
                throw new InvalidOperationException("This vehicle is not available for booking.");

            if (vehicle.AvailabilityStatus != VehicleStatus.Available)
                throw new InvalidOperationException("This vehicle is currently unavailable.");

            if (vehicle.OwnerId == renterId)
                throw new InvalidOperationException("You cannot book your own vehicle.");

            // Check for overlapping confirmed/pending bookings — no double booking
            var hasConflict = _unitOfWork.Bookings
                .GetBookingsByVehicle(dto.VehicleId)
                .Any(b =>
                    b.Status != BookingStatus.Cancelled &&
                    b.Status != BookingStatus.Rejected &&
                    b.StartDate <= dto.EndDate &&
                    b.EndDate >= dto.StartDate);

            if (hasConflict)
                throw new InvalidOperationException("The vehicle is already booked for the selected dates.");

            int days = dto.EndDate.DayNumber - dto.StartDate.DayNumber;
            decimal totalPrice = vehicle.PricePerDay * days;

            var booking = new Booking
            {
                RenterId = renterId,
                VehicleId = dto.VehicleId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = BookingStatus.Pending,
                TotalPrice = totalPrice,
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWork.Bookings.AddBooking(booking);
            _unitOfWork.Save();

            // Notify the vehicle owner
            _notificationServices.SendNotification(
                vehicle.OwnerId,
                $"You have a new booking request from {renter.Name} for {vehicle.Model} ({dto.StartDate} – {dto.EndDate}).",
                "BookingRequest");

            return _mapper.Map<BasicBookingDTO>(booking);
        }

        public BasicBookingDTO ApproveBooking(int bookingId, int ownerId)
        {
            var booking = _unitOfWork.Bookings.GetBookingById(bookingId);
            if (booking == null)
                throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");

            if (booking.Vehicle.OwnerId != ownerId)
                throw new InvalidOperationException("You are not authorised to approve this booking.");

            if (booking.Status != BookingStatus.Pending)
                throw new InvalidOperationException($"Cannot approve a booking that is in '{booking.Status}' status.");

            booking.Status = BookingStatus.Approved;
            _unitOfWork.Bookings.UpdateBooking(booking);

            // Lock the vehicle dates in the calendar
            var calendar = _unitOfWork.AvailabilityCalendars.GetCalendarByVehicle(booking.VehicleId);
            if (calendar != null)
            {
                var blocked = string.IsNullOrWhiteSpace(calendar.BlockedDates)
                    ? new List<string>()
                    : calendar.BlockedDates.Split(',').ToList();

                for (var d = booking.StartDate; d <= booking.EndDate; d = d.AddDays(1))
                    blocked.Add(d.ToString("yyyy-MM-dd"));

                calendar.BlockedDates = string.Join(',', blocked);
                _unitOfWork.AvailabilityCalendars.UpdateCalendar(calendar);
            }

            _unitOfWork.Save();

            // Notify renter
            _notificationServices.SendNotification(
                booking.RenterId,
                $"Your booking for {booking.Vehicle.Model} ({booking.StartDate} – {booking.EndDate}) has been approved.",
                "BookingApproved");

            return _mapper.Map<BasicBookingDTO>(booking);
        }

        public BasicBookingDTO RejectBooking(int bookingId, int ownerId)
        {
            var booking = _unitOfWork.Bookings.GetBookingById(bookingId);
            if (booking == null)
                throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");

            if (booking.Vehicle.OwnerId != ownerId)
                throw new InvalidOperationException("You are not authorised to reject this booking.");

            if (booking.Status != BookingStatus.Pending)
                throw new InvalidOperationException($"Cannot reject a booking that is in '{booking.Status}' status.");

            booking.Status = BookingStatus.Rejected;
            _unitOfWork.Bookings.UpdateBooking(booking);
            _unitOfWork.Save();

            _notificationServices.SendNotification(
                booking.RenterId,
                $"Your booking for {booking.Vehicle.Model} ({booking.StartDate} – {booking.EndDate}) has been rejected.",
                "BookingRejected");

            return _mapper.Map<BasicBookingDTO>(booking);
        }

        public BasicBookingDTO CancelBooking(int bookingId, int userId)
        {
            var booking = _unitOfWork.Bookings.GetBookingById(bookingId);
            if (booking == null)
                throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");

            bool isRenter = booking.RenterId == userId;
            bool isOwner = booking.Vehicle.OwnerId == userId;

            if (!isRenter && !isOwner)
                throw new InvalidOperationException("You are not authorised to cancel this booking.");

            if (booking.Status == BookingStatus.Cancelled)
                throw new InvalidOperationException("Booking is already cancelled.");

            if (booking.Status == BookingStatus.InProgress || booking.Status == BookingStatus.Completed)
                throw new InvalidOperationException($"Cannot cancel a booking that is '{booking.Status}'.");

            // Release blocked dates when cancelling a previously approved booking
            if (booking.Status == BookingStatus.Approved || booking.Status == BookingStatus.Confirmed)
            {
                var calendar = _unitOfWork.AvailabilityCalendars.GetCalendarByVehicle(booking.VehicleId);
                if (calendar != null && !string.IsNullOrWhiteSpace(calendar.BlockedDates))
                {
                    var blockedSet = calendar.BlockedDates.Split(',').ToHashSet();
                    for (var d = booking.StartDate; d <= booking.EndDate; d = d.AddDays(1))
                        blockedSet.Remove(d.ToString("yyyy-MM-dd"));

                    calendar.BlockedDates = string.Join(',', blockedSet);
                    _unitOfWork.AvailabilityCalendars.UpdateCalendar(calendar);
                }
            }

            booking.Status = BookingStatus.Cancelled;
            _unitOfWork.Bookings.UpdateBooking(booking);
            _unitOfWork.Save();

            int notifyUserId = isRenter ? booking.Vehicle.OwnerId : booking.RenterId;
            _notificationServices.SendNotification(
                notifyUserId,
                $"Booking for {booking.Vehicle.Model} ({booking.StartDate} – {booking.EndDate}) has been cancelled.",
                "BookingCancelled");

            return _mapper.Map<BasicBookingDTO>(booking);
        }

        public BasicBookingDTO ConfirmBooking(int bookingId)
        {
            var booking = _unitOfWork.Bookings.GetBookingById(bookingId);
            if (booking == null)
                throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");

            if (booking.Status != BookingStatus.Approved &&
                booking.Status != BookingStatus.PaymentPending)
                throw new InvalidOperationException($"Cannot confirm a booking that is in '{booking.Status}' status.");

            booking.Status = BookingStatus.Confirmed;
            _unitOfWork.Bookings.UpdateBooking(booking);
            _unitOfWork.Save();

            _notificationServices.SendNotification(
                booking.RenterId,
                $"Your booking for {booking.Vehicle.Model} is now confirmed. Enjoy your rental!",
                "BookingConfirmed");

            return _mapper.Map<BasicBookingDTO>(booking);
        }
    }
}