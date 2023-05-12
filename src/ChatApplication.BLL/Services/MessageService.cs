using AutoMapper;
using ChatApplication.BLL.Abstractions.Services;
using ChatApplication.DAL.Data.Interfaces;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Exceptions.NotFound;
using ChatApplication.Shared.Models.Message;
using Microsoft.Identity.Client;

namespace ChatApplication.BLL.Services;

public class MessageService: IMessageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MessageService(
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MessageModel>> GetByChatIdAsync(
        int chatId, 
        CancellationToken cancellationToken = default)
    {
        _ = await _unitOfWork
                .GetRepository<IChatRepository>()
                .GetByIdAsync(chatId, cancellationToken)
            ?? throw new ChatNotFoundException(chatId);
        
        var messages = await _unitOfWork
            .GetRepository<IMessageRepository>()
            .GetByChatIdAsync(chatId, cancellationToken);
        
        return _mapper.Map<IEnumerable<MessageModel>>(messages);
    }

    public async Task<MessageModel> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var message = await _unitOfWork
                          .GetRepository<IMessageRepository>()
                          .GetByIdAsync(id, cancellationToken)
                      ?? throw new MessageNotFoundException(id);
        
        return _mapper.Map<MessageModel>(message);
    }

    public async Task<MessageModel> CreateAsync(
        CreateMessageModel createMessageModel, 
        CancellationToken cancellationToken = default)
    {
        var messageModel = _mapper.Map<Message>(createMessageModel);

        _ = await _unitOfWork
                .GetRepository<IChatRepository>()
                .GetByIdAsync(createMessageModel.ChatId, CancellationToken.None)
            ?? throw new ChatNotFoundException(createMessageModel.ChatId);

        _ = await _unitOfWork
                .GetRepository<IUserRepository>()
                .GetByIdAsync(createMessageModel.SenderId, cancellationToken)
            ?? throw new UserNotFoundException(createMessageModel.SenderId);
        
        await _unitOfWork
            .GetRepository<IMessageRepository>()
            .CreateAsync(messageModel, cancellationToken);
        
        await _unitOfWork
            .SaveChangesAsync(cancellationToken);

        var message = await _unitOfWork
            .GetRepository<IMessageRepository>()
            .GetByIdAsync(messageModel.Id, cancellationToken);

        return _mapper.Map<MessageModel>(message);
    }
}