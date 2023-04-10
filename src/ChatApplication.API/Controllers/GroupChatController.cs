using ChatApplication.BLL.Models.GroupChat;
using ChatApplication.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.API.Controllers;

[ApiController]
[Route("api/group-chats")]
public class GroupChatController: ControllerBase
{
    private readonly IGroupChatService _groupChatService;

    public GroupChatController(IGroupChatService groupChatService)
    {
        _groupChatService = groupChatService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var groupChats = await _groupChatService.GetAllAsync(cancellationToken);
        return Ok(groupChats);
    }
    
    [HttpGet("{id}", Name = nameof(GetById))]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var groupChat = await _groupChatService.GetByIdAsync(id, cancellationToken);
        return Ok(groupChat);
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        CreateGroupChatModel createModel, 
        CancellationToken cancellationToken)
    {
        var groupChat = await _groupChatService.CreateAsync(createModel, cancellationToken);
        return CreatedAtRoute(nameof(GetById), new { id = groupChat.Id }, groupChat);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(
        Guid id,
        UpdateGroupChatModel updateModel,
        CancellationToken cancellationToken)
    {
        updateModel.Id = id;
        await _groupChatService.UpdateAsync(updateModel, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _groupChatService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}