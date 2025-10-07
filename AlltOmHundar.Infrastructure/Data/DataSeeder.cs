using AlltOmHundar.Core.Models;
using AlltOmHundar.Core.Enums;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AlltOmHundar.Infrastructure.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;

        public DataSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Kontrollera om admin redan finns
            if (_context.Users.Any(u => u.Email == "admin@alltomhundar.se"))
                return;

            // Skapa admin-användare
            var adminUser = new User
            {
                Username = "SagaAdmin",
                Email = "saga.admin@alltomhundar.se",
                PasswordHash = HashPassword("Saga123!"),
                Role = UserRole.Admin,
                Bio = "Administratör för AlltOmHundar",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(adminUser);
            await _context.SaveChangesAsync();

            // Skapa en test-användare också
            var testUser = new User
            {
                Username = "RandomDogGuy",
                Email = "random@test.se",
                PasswordHash = HashPassword("Test123!"),
                Role = UserRole.User,
                Bio = "Svensson som älskar hundar",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(testUser);
            await _context.SaveChangesAsync();
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}