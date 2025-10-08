using AlltOmHundar.Core.Models;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByIdAsync(int id);
        Task<User> RegisterUserAsync(string username, string email, string password);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<User?> AuthenticateAsync(string email, string password);
        Task UpdateUserAsync(User user);
    }
}