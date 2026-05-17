using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyShareDAL.Context;
using KeyShareDAL.Interfaces;
using KeyshareDL.Entities;
using KeyshareDL.Enums;
using Microsoft.EntityFrameworkCore;

namespace KeyShareDAL.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly KeyShareContext _context;

        public BookingRepository(KeyShareContext context)
        {
            _context = context;
        }

        public Booking? GetBookingById(int id)
        {
            return _context.Bookings
                .Include(b => b.Renter)
                .Include(b => b.Vehicle)
                    .ThenInclude(v => v.Owner)
                .Include(b => b.Payment)
                .Include(b => b.Review)
                .FirstOrDefault(b => b.BookingId == id);
        }

        public List<Booking> GetAllBookings()
        {
            return _context.Bookings
                .Include(b => b.Renter)
                .Include(b => b.Vehicle)
                .Include(b => b.Payment)
                .ToList();
        }

        public List<Booking> GetBookingsByRenter(int renterId)
        {
            return _context.Bookings
                .Include(b => b.Vehicle)
                    .ThenInclude(v => v.Images)
                .Include(b => b.Payment)
                .Where(b => b.RenterId == renterId)
                .ToList();
        }

        public List<Booking> GetBookingsByVehicle(int vehicleId)
        {
            return _context.Bookings
                .Include(b => b.Renter)
                .Include(b => b.Payment)
                .Where(b => b.VehicleId == vehicleId)
                .ToList();
        }

        public List<Booking> GetBookingsByStatus(BookingStatus status)
        {
            return _context.Bookings
                .Include(b => b.Renter)
                .Include(b => b.Vehicle)
                .Where(b => b.Status == status)
                .ToList();
        }

        public List<Booking> GetPendingBookingsForOwner(int ownerId)
        {
            return _context.Bookings
                .Include(b => b.Renter)
                .Include(b => b.Vehicle)
                .Where(b => b.Vehicle.OwnerId == ownerId && b.Status == BookingStatus.Pending)
                .ToList();
        }

        public Booking AddBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            return booking;
        }

        public Booking UpdateBooking(Booking booking)
        {
            _context.Bookings.Update(booking);
            return booking;
        }

        public Booking DeleteBooking(Booking booking)
        {
            _context.Bookings.Remove(booking);
            return booking;
        }
    }
}
