using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.API.Hubs;

public class VideoHub: Hub
{
    public async Task JoinVideoCallAsync()
    {
        await Clients.All.SendAsync("UserConnected", new Random().Next(0, 100));
    }
}