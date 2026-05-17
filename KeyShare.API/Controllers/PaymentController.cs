using KeyShareBLL.Interfaces;
using KeySharePL.DTOs.PaymentDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeyShare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentServices _paymentServices;
        public PaymentController(IPaymentServices paymentServices) => _paymentServices = paymentServices;

        [HttpGet("{id}")]
        public IActionResult GetPayment(int id) => Ok(_paymentServices.GetPayment(id));

        [HttpGet("booking/{bookingId}")]
        public IActionResult GetByBooking(int bookingId) => Ok(_paymentServices.GetPaymentByBooking(bookingId));

        [HttpPost("simulate")]
        public IActionResult Simulate([FromBody] SimulatePaymentDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(_paymentServices.SimulatePayment(dto));
        }

        [HttpPost("{id}/confirm")]
        [Authorize(Roles = "Admin")]
        public IActionResult Confirm(int id) => Ok(_paymentServices.ConfirmPayment(id));

        [HttpPost("{id}/fail")]
        [Authorize(Roles = "Admin")]
        public IActionResult Fail(int id) => Ok(_paymentServices.FailPayment(id));
    }
}