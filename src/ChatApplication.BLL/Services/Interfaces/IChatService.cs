using ChatApplication.BLL.Models.GroupChat;
using ChatApplication.DAL.Entities;

namespace ChatApplication.BLL.Services.Interfaces;

public interface IGroupChatService
{
    Task<IEnumerable<ChatModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ChatModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ChatModel> CreateAsync(CreateChatModel model, CancellationToken cancellationToken = default);
    Task UpdateAsync(UpdateChatModel model, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}