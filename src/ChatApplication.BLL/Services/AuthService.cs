using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using ChatApplication.BLL.Abstractions.Services;
using ChatApplication.DAL.Entities;
using ChatApplication.Shared.Exceptions.Auth;
using ChatApplication.Shared.Models.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ChatApplication.BLL.Services;

public class AuthService: IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtHandler;

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IMapper mapper, 
        IJwtService jwtHandler)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _jwtHandler = jwtHandler;
    }

    public async Task<JwtModel> LoginAsync(
        LoginModel model,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(model.Email)
            ?? throw new AuthException("Invalid email or password");

        var result = await _signInManager
            .PasswordSignInAsync(user, model.Password, false, false);

        if (!result.Succeeded)
        {
            throw new AuthException("Invalid email or password");
        }

        var claims = await _jwtHandler.GetClaimsAsync(user.Id);
        var signingCredentials = _jwtHandler.GetSigningCredentials();
        var token = _jwtHandler.GenerateToken(signingCredentials, claims);

        return new JwtModel
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
        };
    }
    
    public async Task SignupAsync(
        SignupModel model,
        CancellationToken cancellationToken = default)
    {
        var user = _mapper.Map<User>(model);

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            throw new AuthException(result.ToString());
        }
    }
    
    public async Task ResetPasswordAsync(
        ChangePasswordModel model,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(model.Email)
                   ?? throw new AuthException($"User with email {model.Email} was not found");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

        if (!result.Succeeded)
        {
            throw new AuthException("Failed to reset password");
        }
    }
}