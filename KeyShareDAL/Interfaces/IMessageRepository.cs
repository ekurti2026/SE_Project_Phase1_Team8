using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyshareDL.Entities;
using KeyShareDL.Entities;

namespace KeyShareDAL.Interfaces
{
    public interface IMessageRepository
    {
        Message? GetMessageById(int id);
        List<Message> GetConversation(int userId1, int userId2);
        List<Message> GetUnreadMessages(int receiverId);
        Message AddMessage(Message message);
        Message UpdateMessage(Message message);
        Message DeleteMessage(Message message);
    }
}
