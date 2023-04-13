using AutoMapper;
using ChatApplication.BLL.Exceptions.NotFound;
using ChatApplication.BLL.Models.GroupChat;
using ChatApplication.BLL.Services.Interfaces;
using ChatApplication.DAL.Data.Interfaces;
using ChatApplication.DAL.Entities;

namespace ChatApplication.BLL.Services;

public class ChatService: IChatService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ChatService(
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ChatModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var groupChats = await _unitOfWork.ChatRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ChatModel>>(groupChats);
    }

    public async Task<ChatModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var groupChat = await _unitOfWork.ChatRepository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<ChatModel>(groupChat);
    }

    public async Task<ChatModel> CreateAsync(CreateChatModel model, CancellationToken cancellationToken = default)
    {
        var groupChat = _mapper.Map<Chat>(model);
        
        await _unitOfWork.ChatRepository.CreateAsync(groupChat, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ChatModel>(groupChat);
    }

    public async Task UpdateAsync(UpdateChatModel model, CancellationToken cancellationToken = default)
    {
        var groupChat = await _unitOfWork.ChatRepository.GetByIdAsync(model.Id, cancellationToken)
                        ?? throw new GroupChatNotFoundException("Group chat was not found");

        groupChat.Name = model.Name;

        _unitOfWork.ChatRepository.Update(groupChat);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var groupChat = await _unitOfWork.ChatRepository.GetByIdAsync(id, cancellationToken)
                        ?? throw new GroupChatNotFoundException("Group chat was not found");

        _unitOfWork.ChatRepository.Delete(groupChat);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}