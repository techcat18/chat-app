using ChatApplication.BLL.Models.Chat;
using ChatApplication.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.API.Controllers;

[ApiController]
[Route("api/group-chats")]
public class GroupChatController: ControllerBase
{
    private readonly IChatService _chatService;

    public GroupChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var groupChats = await _chatService.GetAllAsync(cancellationToken);
        return Ok(groupChats);
    }
    
    [HttpGet("{id}", Name = nameof(GetById))]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var groupChat = await _chatService.GetByIdAsync(id, cancellationToken);
        return Ok(groupChat);
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        CreateChatModel createModel, 
        CancellationToken cancellationToken)
    {
        var groupChat = await _chatService.CreateAsync(createModel, cancellationToken);
        return CreatedAtRoute(nameof(GetById), new { id = groupChat.Id }, groupChat);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(
        Guid id,
        UpdateChatModel updateModel,
        CancellationToken cancellationToken)
    {
        updateModel.Id = id;
        await _chatService.UpdateAsync(updateModel, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _chatService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}