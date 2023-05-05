using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.API.Hubs;

[Authorize(AuthenticationSchemes = "Bearer")]
public class VideoHub: Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        
        if (userId != null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }
        
        await base.OnConnectedAsync();
    }

    public async Task JoinVideoCallAsync(string peerId, int chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        await Clients.OthersInGroup(chatId.ToString()).SendAsync("UserConnected", peerId, Context.ConnectionId);
    }

    public async Task CallUserAsync(string peerId, string userId)
    {
        var id = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        await Clients.Group(userId).SendAsync("AnswerCall", id);
    }

    public async Task AcceptUserCallAsync(string peerId, string userId)
    {
        await Clients.Group(userId).SendAsync("CallAccepted", peerId);
    }

    public async Task LeaveVideoCallAsync(int chatId)
    {
        await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}