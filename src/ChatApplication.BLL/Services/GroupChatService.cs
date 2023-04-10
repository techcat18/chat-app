using AutoMapper;
using ChatApplication.BLL.Exceptions.NotFound;
using ChatApplication.BLL.Models.GroupChat;
using ChatApplication.BLL.Services.Interfaces;
using ChatApplication.DAL.Data.Interfaces;
using ChatApplication.DAL.Entities;

namespace ChatApplication.BLL.Services;

public class GroupChatService: IGroupChatService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GroupChatService(
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GroupChatModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var groupChats = await _unitOfWork.GroupChatRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<GroupChatModel>>(groupChats);
    }

    public async Task<GroupChatModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var groupChat = await _unitOfWork.GroupChatRepository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<GroupChatModel>(groupChat);
    }

    public async Task<GroupChatModel> CreateAsync(CreateGroupChatModel model, CancellationToken cancellationToken = default)
    {
        var groupChat = _mapper.Map<GroupChat>(model);
        
        await _unitOfWork.GroupChatRepository.CreateAsync(groupChat, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<GroupChatModel>(groupChat);
    }

    public async Task UpdateAsync(UpdateGroupChatModel model, CancellationToken cancellationToken = default)
    {
        var groupChat = await _unitOfWork.GroupChatRepository.GetByIdAsync(model.Id, cancellationToken)
                        ?? throw new GroupChatNotFoundException("Group chat was not found");

        groupChat.Name = model.Name;

        _unitOfWork.GroupChatRepository.Update(groupChat);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var groupChat = await _unitOfWork.GroupChatRepository.GetByIdAsync(id, cancellationToken)
                        ?? throw new GroupChatNotFoundException("Group chat was not found");

        _unitOfWork.GroupChatRepository.Delete(groupChat);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}