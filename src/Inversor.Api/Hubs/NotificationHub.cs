using Microsoft.AspNetCore.SignalR;

namespace Inversor.Api.Hubs;

/// <summary>
/// Represents a SignalR hub for handling notifications to clients Frontend.
/// </summary>
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
        await base.OnConnectedAsync();
    }
}