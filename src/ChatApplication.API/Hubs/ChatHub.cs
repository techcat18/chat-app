using ChatApplication.BLL.Models.Message;
using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.API.Hubs;

public class ChatHub: Hub
{
    public async Task SendMessageAsync(MessageModel message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}