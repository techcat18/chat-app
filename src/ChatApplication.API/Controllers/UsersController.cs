using ChatApplication.BLL.Abstractions.Services;
using ChatApplication.Shared.Models;
using ChatApplication.Shared.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController: ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var users = await _userService.GetAllAsync(cancellationToken);
        return Ok(users);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllByFilter(
        [FromQuery]UserFilterModel filterModel,
        CancellationToken cancellationToken)
    {
        var users = await _userService.GetAllByFilterAsync(filterModel, cancellationToken);
        return Ok(users);
    }

    [HttpGet("/api/chats/{chatId}/users")]
    public async Task<IActionResult> GetByChatId(
        int chatId,
        CancellationToken cancellationToken)
    {
        var users = await _userService.GetAllByChatIdAsync(chatId, cancellationToken);
        return Ok(users);
    }

    [HttpGet("/api/chats/{chatId}/users/except")]
    public async Task<IActionResult> GetExceptByChatId(
        int chatId,
        CancellationToken cancellationToken)
    {
        var users = await _userService.GetAllExceptByChatIdAsync(chatId, cancellationToken);
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        string id,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetByIdAsync(id, cancellationToken);
        return Ok(user);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut]
    public async Task<IActionResult> Put(
        UpdateUserModel updateUserModel,
        CancellationToken cancellationToken)
    {
        await _userService.UpdateAsync(updateUserModel, cancellationToken);
        return Ok();
    }
}