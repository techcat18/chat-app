using AutoMapper;
using ChatApplication.BLL.Exceptions.NotFound;
using ChatApplication.BLL.Models.Chat;
using ChatApplication.BLL.Services.Interfaces;
using ChatApplication.DAL.Data.Interfaces;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Models;

namespace ChatApplication.BLL.Services;

public class ChatService: IChatService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IChatRepository _chatRepository;
    private readonly IMapper _mapper;

    public ChatService(
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _chatRepository = _unitOfWork.GetRepository<IChatRepository>();
        _mapper = mapper;
    }

    public async Task<IEnumerable<ChatModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var chats = await _chatRepository
            .GetAllAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<ChatModel>>(chats);
    }

    public async Task<PagedList<ChatModel>> GetAllByFilterAsync(
        ChatFilterModel filterModel,
        CancellationToken cancellationToken = default)
    {
        var chats = await _chatRepository
            .GetAllByFilterAsync(filterModel, cancellationToken);
        
        var chatModels = _mapper.Map<IEnumerable<ChatModel>>(chats);

        var totalCount = await _chatRepository
            .GetTotalCountAsync(filterModel, cancellationToken);
        
        var pagedModel = PagedList<ChatModel>
            .ToPagedModel(chatModels, totalCount, filterModel.Page, filterModel.Count);

        return pagedModel;
    }

    public async Task<ChatModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var chat = await _chatRepository
            .GetByIdAsync(id, cancellationToken);
        
        return _mapper.Map<ChatModel>(chat);
    }

    public async Task<ChatModel> CreateAsync(CreateChatModel model, CancellationToken cancellationToken = default)
    {
        var chat = _mapper.Map<Chat>(model);
        
        await _chatRepository
            .CreateAsync(chat, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ChatModel>(chat);
    }

    public async Task UpdateAsync(UpdateChatModel model, CancellationToken cancellationToken = default)
    {
        var chat = await _chatRepository
                       .GetByIdAsync(model.Id, cancellationToken)
                        ?? throw new ChatNotFoundException("Chat was not found");

        chat.Name = model.Name;

        _chatRepository.Update(chat);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var chat = await _chatRepository
                       .GetByIdAsync(id, cancellationToken)
                        ?? throw new ChatNotFoundException("Group chat was not found");

        _chatRepository.Delete(chat);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}