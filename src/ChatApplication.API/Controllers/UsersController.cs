using ChatApplication.BLL.Services.Interfaces;
using ChatApplication.Shared.Models;
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        string id,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetByIdAsync(id, cancellationToken);
        return Ok(user);
    }
}