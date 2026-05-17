using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeySharePL.DTOs.MessageDTOs
{
    public class BasicMessageDTO
    {
        public int MessageId { get; set; }
        public required string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; }
        public int SenderId { get; set; }
        public string? SenderName { get; set; }
        public int ReceiverId { get; set; }
        public string? ReceiverName { get; set; }
    }
}
