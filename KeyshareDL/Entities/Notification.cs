using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyshareDL.Enums;
using KeyShareDL.Entities;

namespace KeyshareDL.Entities
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        public required string Message { get; set; }
        public required string Type { get; set; }

        public NotificationStatus Status { get; set; } = NotificationStatus.Unread;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // FK
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
