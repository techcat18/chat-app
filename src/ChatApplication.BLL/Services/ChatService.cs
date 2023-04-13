using AutoMapper;
using ChatApplication.BLL.Exceptions.NotFound;
using ChatApplication.BLL.Models.Chat;
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
        var chat = await _unitOfWork.ChatRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ChatModel>>(chat);
    }

    public async Task<ChatModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var chat = await _unitOfWork.ChatRepository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<ChatModel>(chat);
    }

    public async Task<ChatModel> CreateAsync(CreateChatModel model, CancellationToken cancellationToken = default)
    {
        var chat = _mapper.Map<Chat>(model);
        
        await _unitOfWork.ChatRepository.CreateAsync(chat, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ChatModel>(chat);
    }

    public async Task UpdateAsync(UpdateChatModel model, CancellationToken cancellationToken = default)
    {
        var chat = await _unitOfWork.ChatRepository.GetByIdAsync(model.Id, cancellationToken)
                        ?? throw new ChatNotFoundException("Group chat was not found");

        chat.Name = model.Name;

        _unitOfWork.ChatRepository.Update(chat);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var chat = await _unitOfWork.ChatRepository.GetByIdAsync(id, cancellationToken)
                        ?? throw new ChatNotFoundException("Group chat was not found");

        _unitOfWork.ChatRepository.Delete(chat);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}