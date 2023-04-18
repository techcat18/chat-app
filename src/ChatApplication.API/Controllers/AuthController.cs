﻿using ChatApplication.BLL.Models.Auth;
using ChatApplication.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController: ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JwtModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        var token = await _authService.LoginAsync(loginModel);
        return Ok(token);
    }

    [HttpPost("signup")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Signup(SignupModel signupModel)
    {
        await _authService.SignupAsync(signupModel);
        return Ok();
    }
}