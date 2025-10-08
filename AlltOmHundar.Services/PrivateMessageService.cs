using AlltOmHundar.Core.Interfaces.Repositories;
using AlltOmHundar.Core.Interfaces.Services;
using AlltOmHundar.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<PrivateMessage>> GetSentMessagesAsync(int userId)
        {
            var messages = await _messageRepository.GetSentMessagesAsync(userId);
            return messages.ToList();
        }
        public async Task<List<PrivateMessage>> GetReceivedMessagesAsync(int userId)
        {
            var messages = await _messageRepository.GetReceivedMessagesAsync(userId);
            return messages.ToList();
        }

        public async Task<PrivateMessage?> GetMessageByIdAsync(int id)
        {
            return await _messageRepository.GetByIdAsync(id);
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _messageRepository.GetUnreadMessageCountAsync(userId);
        }

        public async Task MarkAsReadAsync(int messageId)
        {
            await _messageRepository.MarkAsReadAsync(messageId);

        }
        public async Task<IEnumerable<PrivateMessage>> GetConversationAsync(int user1Id, int user2Id)
        {
            return await _messageRepository.GetMessagesBetweenUsersAsync(user1Id, user2Id);
        }
    }
}