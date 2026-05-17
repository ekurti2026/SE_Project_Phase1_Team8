using System.Security.Claims;
using KeyShareBLL.Interfaces;
using KeySharePL.DTOs.VehicleDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeyShare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleServices _vehicleServices;
        public VehicleController(IVehicleServices vehicleServices) => _vehicleServices = vehicleServices;

        [HttpPost("search")]
        public IActionResult Search([FromBody] SearchVehicleDTO filter)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(_vehicleServices.SearchVehicles(filter));
        }

        [HttpGet("{id}/availability")]
        public IActionResult CheckAvailability(int id, [FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
            => Ok(_vehicleServices.CheckAvailability(id, startDate, endDate));

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetVehicle(int id) => Ok(_vehicleServices.GetVehicle(id));

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll() => Ok(_vehicleServices.GetAllVehicles());

        [HttpGet("my")]
        [Authorize]
        public IActionResult GetMy() => Ok(_vehicleServices.GetVehiclesByOwner(CurrentUserId()));

        [HttpGet("owner/{ownerId}")]
        [Authorize]
        public IActionResult GetByOwner(int ownerId) => Ok(_vehicleServices.GetVehiclesByOwner(ownerId));

        [HttpPost]
        [Authorize(Roles = "Owner,Business,Admin")]
        public IActionResult Create([FromBody] CreateVehicleDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var v = _vehicleServices.CreateVehicle(CurrentUserId(), dto);
            return CreatedAtAction(nameof(GetVehicle), new { id = v.VehicleId }, v);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Owner,Business,Admin")]
        public IActionResult Update(int id, [FromBody] UpdateVehicleDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(_vehicleServices.UpdateVehicle(id, dto));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Owner,Business,Admin")]
        public IActionResult Delete(int id) => Ok(_vehicleServices.DeleteVehicle(id));

        [HttpPost("{id}/publish")]
        [Authorize(Roles = "Owner,Business,Admin")]
        public IActionResult Publish(int id) => Ok(_vehicleServices.PublishVehicle(id));

        [HttpPost("{id}/images")]
        [Authorize(Roles = "Owner,Business,Admin")]
        public IActionResult AddImage(int id, [FromBody] UploadImageDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(_vehicleServices.AddImage(id, dto));
        }

        private int CurrentUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}