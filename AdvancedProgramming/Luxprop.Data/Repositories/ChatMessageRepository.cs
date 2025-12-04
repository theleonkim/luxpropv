using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Repositories
{
    public interface IChatThreadRepository : IRepositoryBase<ChatThread>
    {
        Task<IEnumerable<ChatThread>> GetOpenThreadsAsync();
        Task<IEnumerable<ChatThread>> GetThreadsNeedingAgentAsync();
        Task<ChatThread?> GetByClientEmailAsync(string clientEmail);
        Task<IEnumerable<ChatThread>> GetByStateAsync(string state);
        Task<ChatThread?> GetLatestOpenThreadByEmailAsync(string clientEmail);
    }

    public class ChatThreadRepository : RepositoryBase<ChatThread>, IChatThreadRepository
    {
        public ChatThreadRepository()
        {
            DbSet = DbContext.Set<ChatThread>();
        }

        public async Task<IEnumerable<ChatThread>> GetOpenThreadsAsync()
        {
            return await DbSet
                .Include(ct => ct.ChatMessages)
                .Include(ct => ct.UsuarioCreador)
                .Where(ct => ct.State == "Open")
                .OrderByDescending(ct => ct.CreatedUtc)
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatThread>> GetThreadsNeedingAgentAsync()
        {
            return await DbSet
                .Include(ct => ct.ChatMessages)
                .Include(ct => ct.UsuarioCreador)
                .Where(ct => ct.State == "Open" && ct.NeedsAgent == true)
                .OrderByDescending(ct => ct.CreatedUtc)
                .ToListAsync();
        }

        public async Task<ChatThread?> GetByClientEmailAsync(string clientEmail)
        {
            return await DbSet
                .Include(ct => ct.ChatMessages)
                .Include(ct => ct.UsuarioCreador)
                .FirstOrDefaultAsync(ct => ct.ClientEmail == clientEmail);
        }

        public async Task<IEnumerable<ChatThread>> GetByStateAsync(string state)
        {
            return await DbSet
                .Include(ct => ct.ChatMessages)
                .Include(ct => ct.UsuarioCreador)
                .Where(ct => ct.State == state)
                .OrderByDescending(ct => ct.CreatedUtc)
                .ToListAsync();
        }

        public async Task<ChatThread?> GetLatestOpenThreadByEmailAsync(string clientEmail)
        {
            return await DbSet
                .Include(ct => ct.ChatMessages)
                .Include(ct => ct.UsuarioCreador)
                .Where(ct => ct.State == "Open" && ct.ClientEmail == clientEmail)
                .OrderByDescending(ct => ct.CreatedUtc)
                .FirstOrDefaultAsync();
        }
    }
}
