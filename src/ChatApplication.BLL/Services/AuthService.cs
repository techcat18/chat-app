using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using ChatApplication.BLL.Exceptions.Auth;
using ChatApplication.BLL.Models.Auth;
using ChatApplication.BLL.Services.Interfaces;
using ChatApplication.DAL.Data.Interfaces;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ChatApplication.BLL.Services;

public class AuthService: IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtHandler;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IMapper mapper, 
        IJwtService jwtHandler,
        IUnitOfWork unitOfWork, 
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _jwtHandler = jwtHandler;
        _userRepository = unitOfWork.GetRepository<IUserRepository>();
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
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

        var claims = await _jwtHandler.GetClaimsAsync(user);
        var signingCredentials = _jwtHandler.GetSigningCredentials();
        var token = _jwtHandler.GenerateToken(signingCredentials, claims);

        return new JwtModel()
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

    public async Task ChangeUserInfoAsync(
        ChangeInfoModel changeInfoModel, 
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(changeInfoModel.Id, cancellationToken)
                   ?? throw new AuthException($"User with id {changeInfoModel.Id} was not found");
        
        var currentUserId = GetCurrentUserId();

        if (string.IsNullOrWhiteSpace(currentUserId) || currentUserId != user.Id)
        {
            throw new AuthException("Access denied");
        }

        if (!string.IsNullOrWhiteSpace(changeInfoModel.Email)
            && user.Email != changeInfoModel.Email)
        {
            var result = await _userManager.SetEmailAsync(user, changeInfoModel.Email);

            if (!result.Succeeded)
            {
                throw new AuthException("Failed to change the email");
            }
        }

        if (!string.IsNullOrWhiteSpace(changeInfoModel.FirstName)
            && user.FirstName != changeInfoModel.FirstName)
        {
            user.FirstName = changeInfoModel.FirstName;
        }

        if (!string.IsNullOrWhiteSpace(changeInfoModel.LastName)
            && user.LastName != changeInfoModel.LastName)
        {
            user.LastName = changeInfoModel.LastName;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private string? GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}