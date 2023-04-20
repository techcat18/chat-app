using AutoMapper;
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

    public async Task<PagedList<UserModel>> GetAllByFilterAsync(
        UserFilterModel filterModel, 
        CancellationToken cancellationToken)
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
}