using System.Security.Claims;
using KeyShareBLL.Interfaces;
using KeySharePL.DTOs.MessageDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeyShare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageServices _messageServices;
        public MessageController(IMessageServices messageServices) => _messageServices = messageServices;

        [HttpPost]
        public IActionResult Send([FromBody] SendMessageDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(_messageServices.SendMessage(CurrentUserId(), dto));
        }

        [HttpGet("conversation/{otherUserId}")]
        public IActionResult GetConversation(int otherUserId)
            => Ok(_messageServices.GetConversation(CurrentUserId(), otherUserId));

        [HttpGet("unread")]
        public IActionResult GetUnread() => Ok(_messageServices.GetUnreadMessages(CurrentUserId()));

        [HttpPost("{id}/read")]
        public IActionResult MarkAsRead(int id) => Ok(_messageServices.MarkAsRead(id, CurrentUserId()));

        private int CurrentUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}