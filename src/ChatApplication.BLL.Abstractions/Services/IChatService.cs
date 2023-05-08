using ChatApplication.Shared.Models;
using ChatApplication.Shared.Models.Chat;

namespace ChatApplication.BLL.Abstractions.Services;

public interface IChatService
{
    Task<IEnumerable<ChatModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedList<ChatModel>> GetAllByFilterAsync(
        ChatFilterModel filterModel,
        CancellationToken cancellationToken = default);
    Task<PagedList<ChatModel>> GetAllByUserIdAsync(
        string userId,
        ChatFilterModel filterModel,
        CancellationToken cancellationToken = default);
    Task<ChatModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ChatModel> CreateAsync(CreateChatModel model, CancellationToken cancellationToken = default);
    Task UpdateAsync(UpdateChatModel model, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}