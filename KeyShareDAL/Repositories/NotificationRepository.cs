using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyShareDAL.Context;
using KeyShareDAL.Interfaces;
using KeyshareDL.Entities;
using KeyshareDL.Enums;

using Microsoft.EntityFrameworkCore;

namespace KeyShareDAL.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly KeyShareContext _context;

        public NotificationRepository(KeyShareContext context)
        {
            _context = context;
        }

        public Notification? GetNotificationById(int id)
        {
            return _context.Notifications
                .Include(n => n.User)
                .FirstOrDefault(n => n.NotificationId == id);
        }

        public List<Notification> GetNotificationsByUser(int userId)
        {
            return _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();
        }

        public List<Notification> GetUnreadByUser(int userId)
        {
            return _context.Notifications
                .Where(n => n.UserId == userId && n.Status == NotificationStatus.Unread)
                .ToList();
        }

        public Notification AddNotification(Notification notification)
        {
            _context.Notifications.Add(notification);
            return notification;
        }

        public Notification UpdateNotification(Notification notification)
        {
            _context.Notifications.Update(notification);
            return notification;
        }

        public Notification DeleteNotification(Notification notification)
        {
            _context.Notifications.Remove(notification);
            return notification;
        }
    }
}