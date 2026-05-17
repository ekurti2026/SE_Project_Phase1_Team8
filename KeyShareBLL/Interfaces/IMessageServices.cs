using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeySharePL.DTOs.MessageDTOs;


namespace KeyShareBLL.Interfaces
{
        public interface IMessageServices
        {
            BasicMessageDTO SendMessage(int senderId, SendMessageDTO dto);
            List<BasicMessageDTO> GetConversation(int userId1, int userId2);
            List<BasicMessageDTO> GetUnreadMessages(int receiverId);
            BasicMessageDTO MarkAsRead(int messageId, int userId);
        }
 }