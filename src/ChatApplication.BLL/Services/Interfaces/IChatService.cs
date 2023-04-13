using ChatApplication.BLL.Models.Chat;

namespace ChatApplication.BLL.Services.Interfaces;

public interface IChatService
{
    Task<IEnumerable<ChatModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ChatModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ChatModel> CreateAsync(CreateChatModel model, CancellationToken cancellationToken = default);
    Task UpdateAsync(UpdateChatModel model, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}