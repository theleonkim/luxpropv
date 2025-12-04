namespace Luxprop.Web.Controllers
{
    public record CreateThreadRequest(string ClientName, string ClientEmail, int AgentId);
    public record SendMessageRequest(int ThreadId, string Text, string Sender); // "Client" | "Agent"
    public record ChatMessageDto(int ChatMessageId, int ThreadId, string Text, string Sender, DateTime SentUtc);
    public record ChatThreadDto(int ChatThreadId, string ClientName, string ClientEmail, string State, DateTime CreatedUtc);
}
