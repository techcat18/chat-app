using AutoMapper;
using ChatApplication.BLL.Abstractions.Services;
using ChatApplication.DAL.Data.Interfaces;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Exceptions.BadRequest;
using ChatApplication.Shared.Exceptions.NotFound;
using ChatApplication.Shared.Models;
using ChatApplication.Shared.Models.Chat;

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
        var chats = await _unitOfWork.GetRepository<IChatRepository>()
            .GetAllAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<ChatModel>>(chats);
    }

    public async Task<PagedList<ChatModel>> GetAllByFilterAsync(
        ChatFilterModel filterModel,
        CancellationToken cancellationToken = default)
    {
        var chats = await _unitOfWork.GetRepository<IChatRepository>()
            .GetAllByFilterAsync(filterModel, cancellationToken);
        
        var chatModels = _mapper.Map<IEnumerable<ChatModel>>(chats);

        var totalCount = await _unitOfWork.GetRepository<IChatRepository>()
            .GetTotalCountAsync(filterModel, cancellationToken);
        
        var pagedModel = PagedList<ChatModel>
            .ToPagedModel(chatModels, totalCount, filterModel.Page, filterModel.Count);

        return pagedModel;
    }

    public async Task<PagedList<ChatModel>> GetAllByUserIdAsync(
        string userId, 
        ChatFilterModel filterModel, 
        CancellationToken cancellationToken = default)
    {
        var chats = await _unitOfWork.GetRepository<IChatRepository>()
            .GetAllByUserIdAsync(userId, filterModel, cancellationToken);
        
        var chatModels = _mapper.Map<IEnumerable<ChatModel>>(chats);

        var pagedModel = PagedList<ChatModel>
            .ToPagedModel(chatModels, chats.Count(), filterModel.Page, filterModel.Count);

        return pagedModel;
    }

    public async Task<ChatModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var chat = await _unitOfWork.GetRepository<IChatRepository>()
                       .GetByIdAsync(id, cancellationToken)
                   ?? throw new ChatNotFoundException(id);

        return _mapper.Map<ChatModel>(chat);
    }

    public async Task<ChatModel> CreateAsync(CreateChatModel model, CancellationToken cancellationToken = default)
    {
        var sameNameChat = await _unitOfWork.GetRepository<IChatRepository>()
            .GetByNameAsync(model.Name, cancellationToken);

        if (sameNameChat != null)
        {
            throw new ChatAlreadyExistsException($"Chat with name {model.Name} already exists");
        }
        
        var chat = _mapper.Map<Chat>(model);

        await _unitOfWork.GetRepository<IChatRepository>()
            .CreateAsync(chat, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (model.UserIds.Any())
        {
            var userChats = model.UserIds.Select(x => new UserChat
            {
                ChatId = chat.Id,
                UserId = x,
                DateJoined = DateTime.UtcNow
            });
    
            await _unitOfWork.GetRepository<IUserChatRepository>()
                .CreateRangeAsync(userChats, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        
        return _mapper.Map<ChatModel>(chat);
    }

    public async Task UpdateAsync(UpdateChatModel model, CancellationToken cancellationToken = default)
    {
        var chat = await _unitOfWork.GetRepository<IChatRepository>()
                       .GetByIdAsync(model.Id, cancellationToken)
                        ?? throw new ChatNotFoundException("Chat was not found");

        chat.Name = model.Name;

        _unitOfWork.GetRepository<IChatRepository>().Update(chat);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var chat = await _unitOfWork.GetRepository<IChatRepository>()
                       .GetByIdAsync(id, cancellationToken)
                        ?? throw new ChatNotFoundException("Chat was not found");

        _unitOfWork.GetRepository<IChatRepository>().Delete(chat);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}