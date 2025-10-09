using AlltOmHundar.Core.Interfaces.Repositories;
using AlltOmHundar.Core.Models;
using AlltOmHundar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlltOmHundar.Infrastructure.Repositories
{
    public class PrivateMessageRepository : Repository<PrivateMessage>, IPrivateMessageRepository
    {
        public PrivateMessageRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Override GetByIdAsync för att inkludera Sender och Receiver
        public override async Task<PrivateMessage?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<PrivateMessage>> GetMessagesBetweenUsersAsync(int user1Id, int user2Id)
        {
            return await _dbSet
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => (m.SenderId == user1Id && m.ReceiverId == user2Id) ||
                           (m.SenderId == user2Id && m.ReceiverId == user1Id))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PrivateMessage>> GetReceivedMessagesAsync(int userId)
        {
            return await _dbSet
                .Include(m => m.Sender)
                .Where(m => m.ReceiverId == userId)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PrivateMessage>> GetSentMessagesAsync(int userId)
        {
            return await _dbSet
                .Include(m => m.Receiver)
                .Where(m => m.SenderId == userId)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<int> GetUnreadMessageCountAsync(int userId)
        {
            return await _dbSet
                .CountAsync(m => m.ReceiverId == userId && !m.IsRead);
        }

        public async Task MarkAsReadAsync(int messageId)
        {
            var message = await _dbSet.FindAsync(messageId);
            if (message != null)
            {
                message.IsRead = true;
                message.ReadAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}