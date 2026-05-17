using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyshareDL.Enums;
using KeyShareDL.Enums;

namespace KeySharePL.DTOs.NotificationDTOs
{
    public class BasicNotificationDTO
    {
        public int NotificationId { get; set; }
        public required string Message { get; set; }
        public required string Type { get; set; }
        public NotificationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
    }
}
