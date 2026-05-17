using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KeyShareBLL.Interfaces;
using KeyShareDAL.UnitOfWork;
using KeyshareDL.Entities;
using KeyshareDL.Enums;
using KeyShareDL.Entities;
using KeyShareDL.Enums;
using KeySharePL.DTOs.NotificationDTOs;

namespace KeyShareBLL.Services
{
    public class NotificationServices : INotificationServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<BasicNotificationDTO> GetNotificationsByUser(int userId)
        {
            var user = _unitOfWork.Users.GetUserById(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            var notifications = _unitOfWork.Notifications.GetNotificationsByUser(userId);
            return _mapper.Map<List<BasicNotificationDTO>>(notifications);
        }

        public List<BasicNotificationDTO> GetUnreadByUser(int userId)
        {
            var user = _unitOfWork.Users.GetUserById(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            var notifications = _unitOfWork.Notifications.GetUnreadByUser(userId);
            return _mapper.Map<List<BasicNotificationDTO>>(notifications);
        }

        public BasicNotificationDTO MarkAsRead(int notificationId, int userId)
        {
            var notification = _unitOfWork.Notifications.GetNotificationById(notificationId);
            if (notification == null)
                throw new KeyNotFoundException($"Notification with ID {notificationId} not found.");

            if (notification.UserId != userId)
                throw new InvalidOperationException("You can only mark your own notifications as read.");

            if (notification.Status == NotificationStatus.Read)
                throw new InvalidOperationException("Notification is already marked as read.");

            notification.Status = NotificationStatus.Read;
            _unitOfWork.Notifications.UpdateNotification(notification);
            _unitOfWork.Save();

            return _mapper.Map<BasicNotificationDTO>(notification);
        }

        public void SendNotification(int userId, string message, string type)
        {
            // Silently skip if user does not exist — notifications are fire-and-forget
            var user = _unitOfWork.Users.GetUserById(userId);
            if (user == null) return;

            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                Type = type,
                Status = NotificationStatus.Unread,
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWork.Notifications.AddNotification(notification);
            _unitOfWork.Save();
        }
    }
}
