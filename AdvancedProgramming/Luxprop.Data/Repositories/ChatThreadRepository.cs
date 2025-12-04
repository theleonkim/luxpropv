using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Repositories
{
    public interface IChatMessageRepository : IRepositoryBase<ChatMessage>
    {
        Task<IEnumerable<ChatMessage>> GetByThreadIdAsync(int threadId);
        Task<IEnumerable<ChatMessage>> GetBySenderAsync(string sender);
        Task<IEnumerable<ChatMessage>> GetMessagesAfterIdAsync(int threadId, int messageId);
        Task<ChatMessage?> GetLatestMessageAsync(int threadId);
    }

    public class ChatMessageRepository : RepositoryBase<ChatMessage>, IChatMessageRepository
    {
        public ChatMessageRepository()
        {
            DbSet = DbContext.Set<ChatMessage>();
        }

        public async Task<IEnumerable<ChatMessage>> GetByThreadIdAsync(int threadId)
        {
            return await DbSet
                .Include(cm => cm.ChatThread)
                .Include(cm => cm.Usuario)
                .Where(cm => cm.ChatThreadId == threadId)
                .OrderBy(cm => cm.ChatMessageId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatMessage>> GetBySenderAsync(string sender)
        {
            return await DbSet
                .Include(cm => cm.ChatThread)
                .Include(cm => cm.Usuario)
                .Where(cm => cm.Sender == sender)
                .OrderByDescending(cm => cm.SentUtc)
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatMessage>> GetMessagesAfterIdAsync(int threadId, int messageId)
        {
            return await DbSet
                .Include(cm => cm.ChatThread)
                .Include(cm => cm.Usuario)
                .Where(cm => cm.ChatThreadId == threadId && cm.ChatMessageId > messageId)
                .OrderBy(cm => cm.ChatMessageId)
                .ToListAsync();
        }

        public async Task<ChatMessage?> GetLatestMessageAsync(int threadId)
        {
            return await DbSet
                .Include(cm => cm.ChatThread)
                .Include(cm => cm.Usuario)
                .Where(cm => cm.ChatThreadId == threadId)
                .OrderByDescending(cm => cm.ChatMessageId)
                .FirstOrDefaultAsync();
        }
    }
}
