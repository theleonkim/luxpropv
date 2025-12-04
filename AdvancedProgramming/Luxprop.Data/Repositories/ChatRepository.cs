using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Repositories
{
    public class ChatRepository
    {
        private readonly LuxpropContext _context;

        public ChatRepository(LuxpropContext context)
        {
            _context = context;
        }

        // 🟢 Get all open threads for admin
        public async Task<List<ChatThread>> GetOpenThreadsAsync()
        {
            return await _context.ChatThreads
                .Where(t => t.State == "Open")
                .OrderByDescending(t => t.CreatedUtc)
                .ToListAsync();
        }

        // 🟢 Get messages by thread
        public async Task<List<ChatMessage>> GetMessagesAsync(int threadId)
        {
            return await _context.ChatMessages
                .Where(m => m.ChatThreadId == threadId)
                .OrderBy(m => m.ChatMessageId)
                .ToListAsync();
        }

        // 🟢 Get open thread by email
        public async Task<ChatThread?> GetUserOpenThreadAsync(string email)
        {
            return await _context.ChatThreads
                .FirstOrDefaultAsync(t => t.ClientEmail == email && t.State == "Open");
        }

        // 🟢 Create new thread
        public async Task<int> CreateNewThreadAsync(string name, string email)
        {
            var thread = new ChatThread
            {
                ClientName = name,
                ClientEmail = email,
                State = "Open",
                NeedsAgent = true,
                CreatedUtc = DateTime.UtcNow
            };

            _context.ChatThreads.Add(thread);
            await _context.SaveChangesAsync();
            return thread.ChatThreadId;
        }

        // 🟢 Add client message
        public async Task AddClientMessageAsync(int threadId, string text, string email)
        {
            _context.ChatMessages.Add(new ChatMessage
            {
                ChatThreadId = threadId,
                Text = text.Trim(),
                Sender = "Client",
                SentUtc = DateTime.UtcNow
            });

            var thread = await _context.ChatThreads.FindAsync(threadId);
            if (thread != null)
            {
                thread.NeedsAgent = true;
            }

            await _context.SaveChangesAsync();
        }

        // 🟢 Add bot message
        public async Task AddBotMessageAsync(int threadId, string text)
        {
            _context.ChatMessages.Add(new ChatMessage
            {
                ChatThreadId = threadId,
                Text = text,
                Sender = "Bot",
                SentUtc = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }

        // 🟢 Add agent message
        public async Task AddAgentMessageAsync(int threadId, string text)
        {
            _context.ChatMessages.Add(new ChatMessage
            {
                ChatThreadId = threadId,
                Text = text,
                Sender = "Agent",
                SentUtc = DateTime.UtcNow
            });

            var thread = await _context.ChatThreads.FindAsync(threadId);
            if (thread != null)
            {
                thread.NeedsAgent = false;
            }

            await _context.SaveChangesAsync();
        }

        // 🟢 Close chat thread
        public async Task CloseChatAsync(int threadId)
        {
            var thread = await _context.ChatThreads.FindAsync(threadId);
            if (thread == null) return;

            thread.State = "Closed";
            thread.ClosedUtc = DateTime.UtcNow;
            thread.NeedsAgent = false;

            _context.ChatMessages.Add(new ChatMessage
            {
                ChatThreadId = threadId,
                Text = "Chat has been closed. ✅",
                Sender = "Agent",
                SentUtc = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }
    }
}
