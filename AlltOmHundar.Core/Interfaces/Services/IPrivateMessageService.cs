using AlltOmHundar.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.Interfaces.Services
{
    public interface IPrivateMessageService
    {
        Task<PrivateMessage> SendMessageAsync(int senderId, int receiverId, string content);
        Task<List<PrivateMessage>> GetSentMessagesAsync(int userId);
        Task<List<PrivateMessage>> GetReceivedMessagesAsync(int userId);
        Task<PrivateMessage?> GetMessageByIdAsync(int id);
        Task MarkAsReadAsync(int messageId);
        Task<int> GetUnreadCountAsync(int userId);
    }
}