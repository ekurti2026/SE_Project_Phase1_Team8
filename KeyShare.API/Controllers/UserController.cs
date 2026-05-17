using System.Security.Claims;
using KeyShareBLL.Interfaces;
using KeySharePL.DTOs.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeyShare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices) => _userServices = userServices;

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(_userServices.Register(dto));
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUserDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(_userServices.Login(dto));
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult GetMe() => Ok(_userServices.GetUser(CurrentUserId()));

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUser(int id) => Ok(_userServices.GetUser(id));

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUsers() => Ok(_userServices.GetAllUsers());

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (CurrentUserId() != id && !User.IsInRole("Admin")) return Forbid();
            return Ok(_userServices.UpdateUser(id, dto));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(int id) => Ok(_userServices.DeleteUser(id));

        private int CurrentUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}