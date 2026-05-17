using System.Security.Claims;
using KeyShareBLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeyShare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationServices _notificationServices;
        public NotificationController(INotificationServices notificationServices)
            => _notificationServices = notificationServices;

        [HttpGet]
        public IActionResult GetAll() => Ok(_notificationServices.GetNotificationsByUser(CurrentUserId()));

        [HttpGet("unread")]
        public IActionResult GetUnread() => Ok(_notificationServices.GetUnreadByUser(CurrentUserId()));

        [HttpPost("{id}/read")]
        public IActionResult MarkAsRead(int id) => Ok(_notificationServices.MarkAsRead(id, CurrentUserId()));

        private int CurrentUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}