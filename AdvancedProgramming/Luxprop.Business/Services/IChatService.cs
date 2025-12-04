using Luxprop.Data.Models;

namespace Luxprop.Business.Services
{
    public interface IChatService
    {
        // ===============================
        // MENSAJES
        // ===============================

        /// <summary>
        /// Obtiene todos los mensajes de un hilo.
        /// </summary>
        Task<List<ChatMessage>> GetMessagesAsync(int threadId);

        /// <summary>
        /// Agrega un mensaje enviado por el cliente.
        /// </summary>
        Task AddClientMessageAsync(int threadId, string text, string userEmail);

        /// <summary>
        /// Agrega un mensaje enviado por el bot automático.
        /// </summary>
        Task AddBotMessageAsync(int threadId, string text);

        /// <summary>
        /// Agrega un mensaje enviado por el agente (admin).
        /// </summary>
        Task AddAgentMessageAsync(int threadId, string text);

        Task<int> GetUnreadCountAsync(string role, string userEmail);




        // ===============================
        // 🧵 HILOS (THREADS)
        // ===============================

        /// <summary>
        /// Crea un nuevo hilo de chat para un cliente.
        /// </summary>
        Task<int> CreateNewThreadAsync(string clientName, string clientEmail);

        /// <summary>
        /// Obtiene el hilo de chat abierto de un cliente (si existe).
        /// </summary>
        Task<ChatThread?> GetUserOpenThreadAsync(string email);

        /// <summary>
        /// Obtiene un hilo específico por ID.
        /// </summary>
        Task<ChatThread?> GetThreadByIdAsync(int id);

        /// <summary>
        /// Obtiene todos los hilos activos (abiertos) para el panel del admin.
        /// </summary>
        Task<List<ChatThread>> GetActiveThreadsAsync();

        /// <summary>
        /// Obtiene todos los hilos, sin importar estado.
        /// </summary>
        Task<List<ChatThread>> GetAllThreadsAsync();



        // ===============================
        // ⚙️ CONTROL DE ESTADO
        // ===============================

        /// <summary>
        /// Cierra un chat cambiando su estado a 'Closed'.
        /// </summary>
        Task CloseChatAsync(int threadId);

        /// <summary>
        /// Marca un hilo como "esperando agente".
        /// </summary>
        Task NotifyWaitingAsync(int threadId);
    }



}
