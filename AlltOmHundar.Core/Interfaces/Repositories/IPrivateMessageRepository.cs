using AlltOmHundar.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.Interfaces.Repositories
{
    public interface IPrivateMessageRepository : IRepository<PrivateMessage>
    {
        Task<IEnumerable<PrivateMessage>> GetMessagesBetweenUsersAsync(int user1Id, int user2Id);
        Task<IEnumerable<PrivateMessage>> GetReceivedMessagesAsync(int userId);
        Task<IEnumerable<PrivateMessage>> GetSentMessagesAsync(int userId);
        Task<int> GetUnreadMessageCountAsync(int userId);
        Task MarkAsReadAsync(int messageId);
    }
}