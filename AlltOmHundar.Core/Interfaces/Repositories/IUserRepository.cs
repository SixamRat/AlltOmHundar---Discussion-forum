using AlltOmHundar.Core.Models;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetUserWithPostsAsync(int userId);
    }
}