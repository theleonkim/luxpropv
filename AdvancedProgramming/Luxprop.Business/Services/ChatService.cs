using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Business.Services
{
    public class ChatService : IChatService
    {
        private readonly IDbContextFactory<LuxpropContext> _contextFactory;

        public ChatService(IDbContextFactory<LuxpropContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        // 📩 Obtener todos los mensajes de un hilo
        public async Task<List<ChatMessage>> GetMessagesAsync(int threadId)
        {
            using var db = _contextFactory.CreateDbContext();
            return await db.ChatMessages
                .Where(m => m.ChatThreadId == threadId)
                .OrderBy(m => m.SentUtc)
                .ToListAsync();
        }

        // 🧑‍💬 Agregar mensaje del cliente
        public async Task AddClientMessageAsync(int threadId, string text, string userEmail)
        {
            using var db = _contextFactory.CreateDbContext();

            db.ChatMessages.Add(new ChatMessage
            {
                ChatThreadId = threadId,
                Sender = "Client",
                Text = text,
                SentUtc = DateTime.UtcNow
            });

            // El cliente envió mensaje → necesita agente
            var thread = await db.ChatThreads.FindAsync(threadId);
            if (thread != null)
                thread.NeedsAgent = true;

            await db.SaveChangesAsync();
        }

        // 🤖 Agregar mensaje del bot
        public async Task AddBotMessageAsync(int threadId, string text)
        {
            using var db = _contextFactory.CreateDbContext();

            db.ChatMessages.Add(new ChatMessage
            {
                ChatThreadId = threadId,
                Sender = "Bot",
                Text = text,
                SentUtc = DateTime.UtcNow
            });

            await db.SaveChangesAsync();
        }

        // 🧑‍💼 Agregar mensaje del agente
        public async Task AddAgentMessageAsync(int threadId, string text)
        {
            using var db = _contextFactory.CreateDbContext();

            db.ChatMessages.Add(new ChatMessage
            {
                ChatThreadId = threadId,
                Sender = "Agent",
                Text = text,
                SentUtc = DateTime.UtcNow
            });

            // El agente respondió → ya NO necesita agente
            var thread = await db.ChatThreads.FindAsync(threadId);
            if (thread != null)
                thread.NeedsAgent = false;

            await db.SaveChangesAsync();
        }

        // 🧵 Crear nuevo chat thread
        public async Task<int> CreateNewThreadAsync(string clientName, string clientEmail)
        {
            using var db = _contextFactory.CreateDbContext();

            var newThread = new ChatThread
            {
                ClientName = clientName,
                ClientEmail = clientEmail,
                CreatedUtc = DateTime.UtcNow,
                NeedsAgent = true,   // siempre inicia esperando agente
                State = "Open"
            };

            db.ChatThreads.Add(newThread);
            await db.SaveChangesAsync();

            return newThread.ChatThreadId;
        }

        // 🔍 Obtener chat abierto del cliente
        public async Task<ChatThread?> GetUserOpenThreadAsync(string email)
        {
            using var db = _contextFactory.CreateDbContext();

            return await db.ChatThreads
                .FirstOrDefaultAsync(t => t.ClientEmail == email && t.State == "Open");
        }

        // 🧾 Obtener hilos activos (abiertos)
        public async Task<List<ChatThread>> GetActiveThreadsAsync()
        {
            using var db = _contextFactory.CreateDbContext();

            return await db.ChatThreads
                .Where(t => t.State == "Open")
                .OrderByDescending(t => t.CreatedUtc)
                .ToListAsync();
        }

        // 🔎 Obtener hilo específico
        public async Task<ChatThread?> GetThreadByIdAsync(int id)
        {
            using var db = _contextFactory.CreateDbContext();

            return await db.ChatThreads
                .FirstOrDefaultAsync(t => t.ChatThreadId == id);
        }

        // 🚫 Cerrar chat
        public async Task CloseChatAsync(int threadId)
        {
            using var db = _contextFactory.CreateDbContext();

            var thread = await db.ChatThreads.FindAsync(threadId);
            if (thread == null) return;

            thread.State = "Closed";
            thread.ClosedUtc = DateTime.UtcNow;

            await db.SaveChangesAsync();
        }

        // 📢 Cliente marcó "esperando agente"
        public async Task NotifyWaitingAsync(int threadId)
        {
            using var db = _contextFactory.CreateDbContext();

            var thread = await db.ChatThreads.FindAsync(threadId);
            if (thread != null)
            {
                thread.NeedsAgent = true;
                await db.SaveChangesAsync();
            }
        }

        // Obtener todos los hilos (admin)
        public async Task<List<ChatThread>> GetAllThreadsAsync()
        {
            using var db = _contextFactory.CreateDbContext();

            return await db.ChatThreads
                .OrderByDescending(t => t.CreatedUtc)
                .ToListAsync();
        }

        // Cantidad de chats no leídos / pendientes
        public async Task<int> GetUnreadCountAsync(string role, string userEmail)
        {
            using var db = _contextFactory.CreateDbContext();

            // ADMIN o AGENTE → cantidad de hilos que esperan respuesta
            if (role == "admin" || role == "agent")
            {
                return await db.ChatThreads
                    .CountAsync(t => t.NeedsAgent == true && t.State == "Open");
            }

            // CLIENTE → mensajes del agente aún no vistos
            // *Si IsReadByClient NO existe, NO rompe.*
            return await db.ChatMessages
                .Where(m =>
                    m.Sender == "Agent" &&
                    m.ChatThread.ClientEmail == userEmail
                )
                .CountAsync();
        }
    }
}
