using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeySharePL.DTOs.BookingDTOs;

namespace KeyShareBLL.Interfaces
{
    public interface IBookingServices
    {
        BasicBookingDTO GetBooking(int id);
        List<BasicBookingDTO> GetBookingsByRenter(int renterId);
        List<BasicBookingDTO> GetPendingBookingsForOwner(int ownerId);
        BasicBookingDTO CreateBooking(int renterId, CreateBookingDTO dto);
        BasicBookingDTO ApproveBooking(int bookingId, int ownerId);
        BasicBookingDTO RejectBooking(int bookingId, int ownerId);
        BasicBookingDTO CancelBooking(int bookingId, int userId);
        BasicBookingDTO ConfirmBooking(int bookingId);
    }
}