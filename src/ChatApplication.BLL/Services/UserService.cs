using System.Security.Claims;
using AutoMapper;
using ChatApplication.BLL.Abstractions.Services;
using ChatApplication.DAL.Data.Interfaces;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Exceptions.Auth;
using ChatApplication.Shared.Models;
using ChatApplication.Shared.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ChatApplication.BLL.Services;

public class UserService: IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<User> _userManager;

    public UserService(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IHttpContextAccessor httpContextAccessor,
        UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _userRepository = _unitOfWork.GetRepository<IUserRepository>();
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<IEnumerable<UserModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<UserModel>>(users);
    }

    public async Task<PagedList<UserModel>> GetAllByFilterAsync(
        UserFilterModel filterModel, 
        CancellationToken cancellationToken = default)
    {
        var users = await _userRepository
            .GetByFilterAsync(filterModel, cancellationToken);

        var userModels = _mapper.Map<IEnumerable<UserModel>>(users);
        
        var totalCount = await _userRepository
            .GetTotalCountAsync(filterModel, cancellationToken);
        
        var pagedModel = PagedList<UserModel>
            .ToPagedModel(userModels, totalCount, filterModel.Page, filterModel.Count);

        return pagedModel;
    }

    public async Task<IEnumerable<UserModel>> GetAllByChatIdAsync(
        int chatId, CancellationToken 
            cancellationToken = default)
    {
        var users = await _userRepository
            .GetByChatIdAsync(chatId, cancellationToken);

        return _mapper.Map<IEnumerable<UserModel>>(users);
    }

    public async Task<IEnumerable<UserModel>> GetAllExceptByChatIdAsync(
        int chatId, 
        CancellationToken cancellationToken = default)
    {
        var users = await _userRepository
            .GetAllExceptByChatIdAsync(chatId, cancellationToken);

        return _mapper.Map<IEnumerable<UserModel>>(users);
    }

    public async Task<UserModel> GetByIdAsync(
        string id, 
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken)
                   ?? throw new AuthException($"User with id {id} was not found");

        return _mapper.Map<UserModel>(user);
    }

    public async Task UpdateAsync(UpdateUserModel updateUserModel, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(updateUserModel.Id, cancellationToken)
                   ?? throw new AuthException($"User with id {updateUserModel.Id} was not found");

        var currentUserId = GetCurrentUserId();

        if (string.IsNullOrWhiteSpace(currentUserId) || currentUserId != user.Id)
        {
            throw new AuthException("Access denied");
        }

        if (!string.IsNullOrWhiteSpace(updateUserModel.Email)
            && user.Email != updateUserModel.Email)
        {
            var result = await _userManager.SetEmailAsync(user, updateUserModel.Email);

            if (!result.Succeeded)
            {
                throw new AuthException("Failed to change the email");
            }
        }

        if (!string.IsNullOrWhiteSpace(updateUserModel.FirstName)
            && user.FirstName != updateUserModel.FirstName)
        {
            user.FirstName = updateUserModel.FirstName;
        }

        if (!string.IsNullOrWhiteSpace(updateUserModel.LastName)
            && user.LastName != updateUserModel.LastName)
        {
            user.LastName = updateUserModel.LastName;
        }

        if (updateUserModel.ImageBytes != null)
        {
            var imageString = Convert.ToBase64String(updateUserModel.ImageBytes);
            user.Image = imageString;
        }

        if (updateUserModel.Age != null)
        {
            user.Age = updateUserModel.Age.Value;
        }

        _userRepository.Update(user);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
    
    private string? GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}