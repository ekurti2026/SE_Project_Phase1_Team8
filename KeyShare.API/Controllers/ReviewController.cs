using System.Security.Claims;
using KeyShareBLL.Interfaces;
using KeySharePL.DTOs.ReviewDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeyShare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewServices _reviewServices;
        public ReviewController(IReviewServices reviewServices) => _reviewServices = reviewServices;

        [HttpGet("{id}")]
        public IActionResult GetReview(int id) => Ok(_reviewServices.GetReview(id));

        [HttpGet("vehicle/{vehicleId}")]
        public IActionResult GetByVehicle(int vehicleId) => Ok(_reviewServices.GetReviewsByVehicle(vehicleId));

        [HttpGet("user/{userId}")]
        [Authorize]
        public IActionResult GetByUser(int userId) => Ok(_reviewServices.GetReviewsByUser(userId));

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] CreateReviewDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var r = _reviewServices.CreateReview(CurrentUserId(), dto);
            return CreatedAtAction(nameof(GetReview), new { id = r.ReviewId }, r);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Update(int id, [FromBody] UpdateReviewDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(_reviewServices.UpdateReview(id, CurrentUserId(), dto));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id) => Ok(_reviewServices.DeleteReview(id, CurrentUserId()));

        private int CurrentUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}