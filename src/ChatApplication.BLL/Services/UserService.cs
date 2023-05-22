using AutoMapper;
using ChatApplication.BLL.Abstractions.Services;
using ChatApplication.DAL.Data.Interfaces;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Exceptions.BadRequest;
using ChatApplication.Shared.Exceptions.NotFound;
using ChatApplication.Shared.Models;
using ChatApplication.Shared.Models.User;

namespace ChatApplication.BLL.Services;

public class UserService: IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStorageService _blobStorageService;

    public UserService(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IStorageService blobStorageService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _blobStorageService = blobStorageService;
    }
    
    public async Task<IEnumerable<UserModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _unitOfWork
            .GetRepository<IUserRepository>()
            .GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<UserModel>>(users);
    }

    public async Task<PagedList<UserModel>> GetAllByFilterAsync(
        UserFilterModel filterModel, 
        CancellationToken cancellationToken = default)
    {
        var users = await _unitOfWork
            .GetRepository<IUserRepository>()
            .GetByFilterAsync(filterModel, cancellationToken);

        var userModels = _mapper.Map<IEnumerable<UserModel>>(users);
        
        var totalCount = await _unitOfWork
            .GetRepository<IUserRepository>()
            .GetTotalCountAsync(filterModel, cancellationToken);
        
        var pagedModel = PagedList<UserModel>
            .ToPagedModel(userModels, totalCount, filterModel.Page, filterModel.Count);

        return pagedModel;
    }

    public async Task<IEnumerable<UserModel>> GetAllByChatIdAsync(
        int chatId, CancellationToken 
            cancellationToken = default)
    {
        _ = await _unitOfWork
                .GetRepository<IChatRepository>()
                .GetByIdAsync(chatId, cancellationToken)
            ?? throw new ChatNotFoundException(chatId);
        
        var users = await _unitOfWork
            .GetRepository<IUserRepository>()
            .GetByChatIdAsync(chatId, cancellationToken);

        return _mapper.Map<IEnumerable<UserModel>>(users);
    }

    public async Task<IEnumerable<UserModel>> GetAllExceptByChatIdAsync(
        int chatId, 
        CancellationToken cancellationToken = default)
    {
        _ = await _unitOfWork
                .GetRepository<IChatRepository>()
                .GetByIdAsync(chatId, cancellationToken)
            ?? throw new ChatNotFoundException(chatId);
        
        var users = await _unitOfWork
            .GetRepository<IUserRepository>()
            .GetAllExceptByChatIdAsync(chatId, cancellationToken);

        return _mapper.Map<IEnumerable<UserModel>>(users);
    }

    public async Task<UserModel> GetByIdAsync(
        string id, 
        CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork
                       .GetRepository<IUserRepository>()
                       .GetByIdAsync(id, cancellationToken)
                   ?? throw new UserNotFoundException(id);

        user.Image += "?" + await _blobStorageService.GetSasTokenAsync("user-photos", user.Email);

        return _mapper.Map<UserModel>(user);
    }

    public async Task UpdateAsync(
        UpdateUserModel updateUserModel, 
        CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork
                       .GetRepository<IUserRepository>()
                       .GetByIdAsync(updateUserModel.Id, cancellationToken)
                   ?? throw new UserNotFoundException(updateUserModel.Id);

        if (!string.IsNullOrWhiteSpace(updateUserModel.Email)
            && user.Email != updateUserModel.Email)
        {
            var existingUser = await _unitOfWork
                .GetRepository<IUserRepository>()
                .GetByEmailAsync(updateUserModel.Email, cancellationToken);

            if (existingUser != null)
            {
                throw new UserAlreadyExistsException($"User with email {updateUserModel.Email} already exists");
            }

            user.Email = updateUserModel.Email;
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
            var stream = new MemoryStream(updateUserModel.ImageBytes);

            var blobUri = 
                await _blobStorageService.UploadAsync(stream, "user-photos", user.Email);

            user.Image = blobUri;
        }

        if (updateUserModel.Age != null)
        {
            user.Age = updateUserModel.Age.Value;
        }

        _unitOfWork
            .GetRepository<IUserRepository>()
            .Update(user);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}