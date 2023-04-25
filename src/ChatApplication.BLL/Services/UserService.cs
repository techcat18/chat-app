using AutoMapper;
using ChatApplication.BLL.Exceptions.Auth;
using ChatApplication.BLL.Models.User;
using ChatApplication.BLL.Services.Interfaces;
using ChatApplication.DAL.Data.Interfaces;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Models;

namespace ChatApplication.BLL.Services;

public class UserService: IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userRepository = _unitOfWork.GetRepository<IUserRepository>();
        _mapper = mapper;
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

    public async Task<UserModel> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken)
                   ?? throw new AuthException($"User with id {id} was not found");

        return _mapper.Map<UserModel>(user);
    }
}