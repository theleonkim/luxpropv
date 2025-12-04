using Microsoft.AspNetCore.SignalR;

namespace Luxprop.Hubs
{
    public class ChatHub : Hub
    {
        public Task JoinThread(string threadId) =>
            Groups.AddToGroupAsync(Context.ConnectionId, $"thread:{threadId}");

        public Task LeaveThread(string threadId) =>
            Groups.RemoveFromGroupAsync(Context.ConnectionId, $"thread:{threadId}");

        public Task JoinAgent(string agentId) =>
            Groups.AddToGroupAsync(Context.ConnectionId, $"agent:{agentId}");
    }
}
