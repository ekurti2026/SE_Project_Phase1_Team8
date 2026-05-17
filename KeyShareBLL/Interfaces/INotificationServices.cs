using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeySharePL.DTOs.NotificationDTOs;

namespace KeyShareBLL.Interfaces
{
    public interface INotificationServices
    {
        List<BasicNotificationDTO> GetNotificationsByUser(int userId);
        List<BasicNotificationDTO> GetUnreadByUser(int userId);
        BasicNotificationDTO MarkAsRead(int notificationId, int userId);
        void SendNotification(int userId, string message, string type);
    }
}