using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyshareDL.Entities;
using KeyShareDL.Entities;

namespace KeyShareDAL.Interfaces
{
    public interface INotificationRepository
    {
        Notification? GetNotificationById(int id);
        List<Notification> GetNotificationsByUser(int userId);
        List<Notification> GetUnreadByUser(int userId);
        Notification AddNotification(Notification notification);
        Notification UpdateNotification(Notification notification);
        Notification DeleteNotification(Notification notification);
    }
}