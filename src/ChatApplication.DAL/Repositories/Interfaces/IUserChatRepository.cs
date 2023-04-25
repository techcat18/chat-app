using ChatApplication.DAL.Entities;

namespace ChatApplication.DAL.Repositories.Interfaces;

public interface IUserChatRepository
{
    Task AddUserToChatAsync(UserChat userChat, CancellationToken cancellationToken = default);
    Task AddUsersToChatAsync(
        IEnumerable<UserChat> userChats,
        CancellationToken cancellationToken = default);
}