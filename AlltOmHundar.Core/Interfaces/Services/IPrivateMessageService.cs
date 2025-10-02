using AlltOmHundar.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.Interfaces.Services
{
    public interface IPrivateMessageService
    {
        Task<PrivateMessage> SendMessageAsync(int senderId, int receiverId, string content);
        Task<IEnumerable<PrivateMessage>> GetConversationAsync(int user1Id, int user2Id);
        Task<IEnumerable<PrivateMessage>> GetReceivedMessagesAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
        Task<bool> MarkAsReadAsync(int messageId, int userId);
    }
}