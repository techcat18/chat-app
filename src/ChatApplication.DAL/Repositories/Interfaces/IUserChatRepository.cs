using ChatApplication.DAL.Entities;

namespace ChatApplication.DAL.Repositories.Interfaces;

public interface IUserChatRepository
{
    Task CreateAsync(UserChat userChat, CancellationToken cancellationToken = default);
    Task CreateRangeAsync(
        IEnumerable<UserChat> userChats,
        CancellationToken cancellationToken = default);
}