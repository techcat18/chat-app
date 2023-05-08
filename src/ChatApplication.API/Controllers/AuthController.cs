using ChatApplication.BLL.Abstractions.Services;
using ChatApplication.Shared.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
[Route("api/auth")]
public class AuthController: ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JwtModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        var token = await _authService.LoginAsync(loginModel);
        return Ok(token);
    }

    [AllowAnonymous]
    [HttpPost("signup")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Signup(SignupModel signupModel)
    {
        await _authService.SignupAsync(signupModel);
        return Ok();
    }

    [HttpPut("changePassword")]
    public async Task<IActionResult> ChangePassword(
        ChangePasswordModel changePasswordModel)
    {
        await _authService.ResetPasswordAsync(changePasswordModel);
        return NoContent();
    }
}