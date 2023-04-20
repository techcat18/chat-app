using ChatApplication.BLL.Models.Chat;
using ChatApplication.BLL.Services.Interfaces;
using ChatApplication.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.API.Controllers;

[ApiController]
[Route("api/chats")]
public class ChatsController: ControllerBase
{
    private readonly IChatService _chatService;

    public ChatsController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("all")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var chats = await _chatService.GetAllAsync(cancellationToken);
        return Ok(chats);
    }

    [HttpGet]
    public async Task<IActionResult> GetByFilter(
        [FromQuery]ChatFilterModel model, 
        CancellationToken cancellationToken)
    {
        var chats = await _chatService.GetAllByFilterAsync(model, cancellationToken);
        return Ok(chats);
    }
    
    [HttpGet("{id}", Name = nameof(GetById))]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var chat = await _chatService.GetByIdAsync(id, cancellationToken);
        return Ok(chat);
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        CreateChatModel createModel, 
        CancellationToken cancellationToken)
    {
        var chat = await _chatService.CreateAsync(createModel, cancellationToken);
        return CreatedAtRoute(nameof(GetById), new { id = chat.Id }, chat);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(
        int id,
        UpdateChatModel updateModel,
        CancellationToken cancellationToken)
    {
        updateModel.Id = id;
        await _chatService.UpdateAsync(updateModel, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await _chatService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}