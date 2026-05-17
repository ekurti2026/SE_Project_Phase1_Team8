using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyshareDL.Entities;
using KeyshareDL.Enums;
using KeyShareDL.Enums;

namespace KeyShareDAL.Interfaces
{
    public interface IBookingRepository
    {
        Booking? GetBookingById(int id);
        List<Booking> GetAllBookings();
        List<Booking> GetBookingsByRenter(int renterId);
        List<Booking> GetBookingsByVehicle(int vehicleId);
        List<Booking> GetBookingsByStatus(BookingStatus status);
        List<Booking> GetPendingBookingsForOwner(int ownerId);
        Booking AddBooking(Booking booking);
        Booking UpdateBooking(Booking booking);
        Booking DeleteBooking(Booking booking);
    }
}
