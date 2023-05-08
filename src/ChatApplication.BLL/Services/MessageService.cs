using AutoMapper;
using ChatApplication.BLL.Abstractions.Services;
using ChatApplication.DAL.Data.Interfaces;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Models.Message;

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

    public async Task<IEnumerable<MessageModel>> GetMessagesByChatIdAsync(
        int chatId, 
        CancellationToken cancellationToken = default)
    {
        var messages = await _unitOfWork.GetRepository<IMessageRepository>().GetByChatIdAsync(chatId, cancellationToken);
        return _mapper.Map<IEnumerable<MessageModel>>(messages);
    }

    public async Task<MessageModel> GetMessageByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var message = await _unitOfWork.GetRepository<IMessageRepository>().GetByIdAsync(id, cancellationToken);
        return _mapper.Map<MessageModel>(message);
    }

    public async Task<MessageModel> CreateMessageAsync(
        CreateMessageModel createMessageModel, 
        CancellationToken cancellationToken = default)
    {
        var messageModel = _mapper.Map<Message>(createMessageModel);
        
        await _unitOfWork.GetRepository<IMessageRepository>().CreateAsync(messageModel, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var message = await _unitOfWork.GetRepository<IMessageRepository>().GetByIdAsync(messageModel.Id, cancellationToken);

        return _mapper.Map<MessageModel>(message);
    }
}