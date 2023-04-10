using ChatApplication.BLL.Models.GroupChat;
using ChatApplication.DAL.Entities;

namespace ChatApplication.BLL.Services.Interfaces;

public interface IGroupChatService
{
    Task<IEnumerable<GroupChatModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<GroupChatModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<GroupChatModel> CreateAsync(CreateGroupChatModel model, CancellationToken cancellationToken = default);
    Task UpdateAsync(UpdateGroupChatModel model, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}