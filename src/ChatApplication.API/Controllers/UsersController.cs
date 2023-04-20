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

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery]UserFilterModel filterModel,
        CancellationToken cancellationToken)
    {
        var users = await _userService.GetAllByFilterAsync(filterModel, cancellationToken);
        return Ok(users);
    }
}