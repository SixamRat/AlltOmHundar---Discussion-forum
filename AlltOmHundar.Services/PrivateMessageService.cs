using AlltOmHundar.Core.Interfaces.Repositories;
using AlltOmHundar.Core.Interfaces.Services;
using AlltOmHundar.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlltOmHundar.Services
{
    public class PrivateMessageService : IPrivateMessageService
    {
        private readonly IPrivateMessageRepository _messageRepository;

        public PrivateMessageService(IPrivateMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<PrivateMessage> SendMessageAsync(int senderId, int receiverId, string content)
        {
            var message = new PrivateMessage
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            await _messageRepository.AddAsync(message);
            return message;
        }

        public async Task<IEnumerable<PrivateMessage>> GetConversationAsync(int user1Id, int user2Id)
        {
            return await _messageRepository.GetMessagesBetweenUsersAsync(user1Id, user2Id);
        }

        public async Task<IEnumerable<PrivateMessage>> GetReceivedMessagesAsync(int userId)
        {
            return await _messageRepository.GetReceivedMessagesAsync(userId);
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _messageRepository.GetUnreadMessageCountAsync(userId);
        }

        public async Task<bool> MarkAsReadAsync(int messageId, int userId)
        {
            var message = await _messageRepository.GetByIdAsync(messageId);
            if (message == null || message.ReceiverId != userId)
                return false;

            message.IsRead = true;
            message.ReadAt = DateTime.UtcNow;

            await _messageRepository.UpdateAsync(message);
            return true;
        }
    }
}