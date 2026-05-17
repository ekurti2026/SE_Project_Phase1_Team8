using System.Security.Claims;
using KeyShareBLL.Interfaces;
using KeySharePL.DTOs.BookingDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeyShare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingServices _bookingServices;
        public BookingController(IBookingServices bookingServices) => _bookingServices = bookingServices;

        [HttpGet("{id}")]
        public IActionResult GetBooking(int id) => Ok(_bookingServices.GetBooking(id));

        [HttpGet("my")]
        public IActionResult GetMyBookings() => Ok(_bookingServices.GetBookingsByRenter(CurrentUserId()));

        [HttpGet("my/pending")]
        public IActionResult GetMyPending() => Ok(_bookingServices.GetPendingBookingsForOwner(CurrentUserId()));

        [HttpGet("renter/{renterId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetByRenter(int renterId) => Ok(_bookingServices.GetBookingsByRenter(renterId));

        [HttpPost]
        public IActionResult Create([FromBody] CreateBookingDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var b = _bookingServices.CreateBooking(CurrentUserId(), dto);
            return CreatedAtAction(nameof(GetBooking), new { id = b.BookingId }, b);
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "Owner,Business,Admin")]
        public IActionResult Approve(int id) => Ok(_bookingServices.ApproveBooking(id, CurrentUserId()));

        [HttpPost("{id}/reject")]
        [Authorize(Roles = "Owner,Business,Admin")]
        public IActionResult Reject(int id) => Ok(_bookingServices.RejectBooking(id, CurrentUserId()));

        [HttpPost("{id}/cancel")]
        public IActionResult Cancel(int id) => Ok(_bookingServices.CancelBooking(id, CurrentUserId()));

        [HttpPost("{id}/confirm")]
        [Authorize(Roles = "Admin")]
        public IActionResult Confirm(int id) => Ok(_bookingServices.ConfirmBooking(id));

        private int CurrentUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}