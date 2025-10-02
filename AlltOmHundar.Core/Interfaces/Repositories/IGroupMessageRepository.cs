using AlltOmHundar.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.Interfaces.Repositories
{
    public interface IGroupMessageRepository : IRepository<GroupMessage>
    {
        Task<IEnumerable<GroupMessage>> GetMessagesByGroupAsync(int groupId);
    }
}