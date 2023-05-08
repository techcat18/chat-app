using ChatApplication.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.API.Hubs;

[Authorize(AuthenticationSchemes = "Bearer")]
public class ChatHub: Hub
{
    private readonly IMessageService _messageService;

    public ChatHub(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public async Task SendMessageAsync(int messageId, int chatId)
    {
        var message = await _messageService.GetMessageByIdAsync(messageId);
        await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", message);
        await SendNotificationAsync(chatId);
    }

    private async Task SendNotificationAsync(int chatId)
    {
        await Clients.OthersInGroup(chatId.ToString()).SendAsync("ReceiveNotification", chatId);
    }

    public async Task JoinGroupAsync(int chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    }

    public async Task JoinVideoCallAsync(int chatId)
    {
        await Clients.All.SendAsync("UserConnected");
    }
}