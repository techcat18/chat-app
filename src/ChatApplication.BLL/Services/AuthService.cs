using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Text;
using AutoMapper;
using ChatApplication.BLL.Exceptions.Auth;
using ChatApplication.BLL.Models.Auth;
using ChatApplication.BLL.Services.Interfaces;
using ChatApplication.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace ChatApplication.BLL.Services;

public class AuthService: IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtHandler;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IMapper mapper, 
        IJwtService jwtHandler,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _jwtHandler = jwtHandler;
        _configuration = configuration;
    }

    public async Task<JwtModel> LoginAsync(LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            throw new AuthException("Invalid email or password");
        }

        var result = await _signInManager
            .PasswordSignInAsync(user, model.Password, false, false);

        if (!result.Succeeded)
        {
            throw new AuthException("Invalid email or password");
        }

        var claims = await _jwtHandler.GetClaimsAsync(user);
        var signingCredentials = _jwtHandler.GetSigningCredentials();
        var token = _jwtHandler.GenerateToken(signingCredentials, claims);

        return new JwtModel()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
        };
    }
    
    public async Task SignupAsync(SignupModel model)
    {
        var user = _mapper.Map<User>(model);

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            throw new AuthException(result.ToString());
        }
    }
}