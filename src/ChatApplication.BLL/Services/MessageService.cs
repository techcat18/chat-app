using AutoMapper;
using ChatApplication.BLL.Models.Message;
using ChatApplication.BLL.Services.Interfaces;
using ChatApplication.DAL.Data.Interfaces;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;

namespace ChatApplication.BLL.Services;

public class MessageService: IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MessageService(
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _messageRepository = unitOfWork.GetRepository<IMessageRepository>();
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MessageModel>> GetMessagesByChatIdAsync(
        int chatId, 
        CancellationToken cancellationToken = default)
    {
        var messages = await _messageRepository.GetByChatIdAsync(chatId, cancellationToken);
        return _mapper.Map<IEnumerable<MessageModel>>(messages);
    }

    public async Task<MessageModel> CreateMessageAsync(
        CreateMessageModel createMessageModel, 
        CancellationToken cancellationToken = default)
    {
        var messageModel = _mapper.Map<Message>(createMessageModel);
        
        await _messageRepository.CreateAsync(messageModel, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var message = await _messageRepository.GetByIdAsync(messageModel.Id, cancellationToken);

        return _mapper.Map<MessageModel>(message);
    }
}