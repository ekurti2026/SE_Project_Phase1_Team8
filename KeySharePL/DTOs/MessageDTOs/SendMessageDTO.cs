using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeySharePL.DTOs.MessageDTOs
{
    public class SendMessageDTO
    {
        public int ReceiverId { get; set; }
        public required string Content { get; set; }
    }
}
