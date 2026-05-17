using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using KeyShareBLL.Interfaces;
using KeyShareDAL.UnitOfWork;
using KeyshareDL.Entities;
using KeyShareDL.Entities;
using KeySharePL.DTOs.MessageDTOs;

namespace KeyShareBLL.Services
{
    public class MessageServices : IMessageServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessageServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public BasicMessageDTO SendMessage(int senderId, SendMessageDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Content))
                throw new ArgumentException("Message content cannot be empty.");

            if (senderId == dto.ReceiverId)
                throw new ArgumentException("You cannot send a message to yourself.");

            var sender = _unitOfWork.Users.GetUserById(senderId);
            if (sender == null)
                throw new KeyNotFoundException($"Sender with ID {senderId} not found.");

            var receiver = _unitOfWork.Users.GetUserById(dto.ReceiverId);
            if (receiver == null)
                throw new KeyNotFoundException($"Receiver with ID {dto.ReceiverId} not found.");

            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = dto.ReceiverId,
                Content = dto.Content,
                IsRead = false,
                SentAt = DateTime.UtcNow
            };

            _unitOfWork.Messages.AddMessage(message);
            _unitOfWork.Save();

            // Re-fetch with includes so AutoMapper can resolve names
            var saved = _unitOfWork.Messages.GetMessageById(message.MessageId)!;
            return _mapper.Map<BasicMessageDTO>(saved);
        }

        public List<BasicMessageDTO> GetConversation(int userId1, int userId2)
        {
            var user1 = _unitOfWork.Users.GetUserById(userId1);
            if (user1 == null)
                throw new KeyNotFoundException($"User with ID {userId1} not found.");

            var user2 = _unitOfWork.Users.GetUserById(userId2);
            if (user2 == null)
                throw new KeyNotFoundException($"User with ID {userId2} not found.");

            var messages = _unitOfWork.Messages.GetConversation(userId1, userId2);
            return _mapper.Map<List<BasicMessageDTO>>(messages);
        }

        public List<BasicMessageDTO> GetUnreadMessages(int receiverId)
        {
            var user = _unitOfWork.Users.GetUserById(receiverId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {receiverId} not found.");

            var messages = _unitOfWork.Messages.GetUnreadMessages(receiverId);
            return _mapper.Map<List<BasicMessageDTO>>(messages);
        }

        public BasicMessageDTO MarkAsRead(int messageId, int userId)
        {
            var message = _unitOfWork.Messages.GetMessageById(messageId);
            if (message == null)
                throw new KeyNotFoundException($"Message with ID {messageId} not found.");

            if (message.ReceiverId != userId)
                throw new InvalidOperationException("You can only mark your own received messages as read.");

            if (message.IsRead)
                throw new InvalidOperationException("Message is already marked as read.");

            message.IsRead = true;
            _unitOfWork.Messages.UpdateMessage(message);
            _unitOfWork.Save();

            return _mapper.Map<BasicMessageDTO>(message);
        }
    }
}