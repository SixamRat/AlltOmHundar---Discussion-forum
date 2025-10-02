using AlltOmHundar.Core.Models;
using System.Threading.Tasks;

namespace AlltOmHundar.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User> RegisterUserAsync(string username, string email, string password);
        Task<User?> AuthenticateAsync(string email, string password);
        Task<bool> UpdateProfileAsync(int userId, string? bio, string? profileImageUrl);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
    }
}