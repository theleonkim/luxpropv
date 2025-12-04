using Luxprop.Data.Models;
using Luxprop.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Controllers
{
    // DTOs locales al controlador (puedes moverlos a un archivo si prefieres)
    public record CreateThreadRequest(string ClientName, string ClientEmail, int AgentId);
    public record SendMessageRequest(int ThreadId, string Text, string Sender); // "Client" | "Agent"
    public record ChatMessageDto(int ChatMessageId, int ThreadId, string Text, string Sender, DateTime SentUtc);
    public record ChatThreadDto(int ChatThreadId, string ClientName, string ClientEmail, string State, DateTime CreatedUtc);

    [ApiController]
    [Route("api/chats")]
    public class ChatController : ControllerBase
    {
        private readonly LuxpropContext _db;
        private readonly IHubContext<ChatHub> _hub;

        public ChatController(LuxpropContext db, IHubContext<ChatHub> hub)
        {
            _db = db;
            _hub = hub;
        }

        // Cliente inicia un chat (crea hilo)
        [HttpPost("threads")]
        public async Task<ActionResult<ChatThreadDto>> CreateThread([FromBody] CreateThreadRequest req)
        {
            var thread = new ChatThread
            {
                ClientName = req.ClientName,
                ClientEmail = req.ClientEmail,
                State = "Open",
                CreatedUtc = DateTime.UtcNow
            };
            _db.ChatThreads.Add(thread);
            await _db.SaveChangesAsync();

            await _hub.Clients.Group($"agent:{req.AgentId}")
                .SendAsync("NewThread", new { threadId = thread.ChatThreadId, clientName = thread.ClientName });

            return new ChatThreadDto(thread.ChatThreadId, thread.ClientName!, thread.ClientEmail!, thread.State!, thread.CreatedUtc);
        }

        // Enviar mensaje
        [HttpPost("messages")]
        public async Task<ActionResult<ChatMessageDto>> SendMessage([FromBody] SendMessageRequest req)
        {
            var thread = await _db.ChatThreads.FindAsync(req.ThreadId);
            if (thread is null) return NotFound();

            var msg = new ChatMessage
            {
                ChatThreadId = req.ThreadId,
                Text = req.Text,
                Sender = req.Sender,
                SentUtc = DateTime.UtcNow
            };
            _db.ChatMessages.Add(msg);
            await _db.SaveChangesAsync();

            var dto = new ChatMessageDto(msg.ChatMessageId, msg.ChatThreadId, msg.Text!, msg.Sender!, msg.SentUtc);

            await _hub.Clients.Group($"thread:{req.ThreadId}")
                .SendAsync("ReceiveMessage", dto);

            return dto;
        }

        // Cargar mensajes del hilo
        [HttpGet("threads/{threadId:int}/messages")]
        public async Task<ActionResult<IEnumerable<ChatMessageDto>>> GetMessages([FromRoute] int threadId)
        {
            var list = await _db.ChatMessages
                .Where(m => m.ChatThreadId == threadId)
                .OrderBy(m => m.SentUtc)
                .Select(m => new ChatMessageDto(m.ChatMessageId, m.ChatThreadId, m.Text!, m.Sender!, m.SentUtc))
                .ToListAsync();

            return list;
        }
    }
}
