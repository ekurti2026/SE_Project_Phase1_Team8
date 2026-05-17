using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyShareDAL.Context;
using KeyShareDAL.Interfaces;
using KeyshareDL.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeyShareDAL.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly KeyShareContext _context;

        public MessageRepository(KeyShareContext context)
        {
            _context = context;
        }

        public Message? GetMessageById(int id)
        {
            return _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .FirstOrDefault(m => m.MessageId == id);
        }

        public List<Message> GetConversation(int userId1, int userId2)
        {
            return _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m =>
                    (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                    (m.SenderId == userId2 && m.ReceiverId == userId1))
                .OrderBy(m => m.SentAt)
                .ToList();
        }

        public List<Message> GetUnreadMessages(int receiverId)
        {
            return _context.Messages
                .Include(m => m.Sender)
                .Where(m => m.ReceiverId == receiverId && !m.IsRead)
                .ToList();
        }

        public Message AddMessage(Message message)
        {
            _context.Messages.Add(message);
            return message;
        }

        public Message UpdateMessage(Message message)
        {
            _context.Messages.Update(message);
            return message;
        }

        public Message DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
            return message;
        }
    }
}
