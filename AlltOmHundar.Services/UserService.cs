using AlltOmHundar.Core.Enums;
using AlltOmHundar.Core.Interfaces.Repositories;
using AlltOmHundar.Core.Interfaces.Services;
using AlltOmHundar.Core.Models;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AlltOmHundar.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> RegisterUserAsync(string username, string email, string password)
        {
            var passwordHash = HashPassword(password);

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                Role = UserRole.User,  // <-- Använd Role istället för IsAdmin
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var passwordHash = HashPassword(password);
            return user.PasswordHash == passwordHash;
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return null;

            var isValidPassword = await CheckPasswordAsync(user, password);
            if (!isValidPassword)
                return null;

            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}