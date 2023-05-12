using ChatApplication.BLL.Abstractions.Services;
using ChatApplication.Shared.Models.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
[Route("api/messages")]
public class MessagesController: ControllerBase
{
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet("/api/chats/{chatId}/messages")]
    public async Task<IActionResult> GetByChatId(
        int chatId,
        CancellationToken cancellationToken)
    {
        var messages = await _messageService.GetByChatIdAsync(chatId, cancellationToken);
        return Ok(messages);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateMessageModel createMessageModel,
        CancellationToken cancellationToken)
    {
        var message = await _messageService.CreateAsync(createMessageModel, cancellationToken);
        return Ok(message);
    }
}