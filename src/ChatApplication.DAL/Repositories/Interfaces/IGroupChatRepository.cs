using ChatApplication.DAL.Entities;

namespace ChatApplication.DAL.Repositories.Interfaces;

public interface IGroupChatRepository: IGenericRepository<Guid, GroupChat>
{
    Task<GroupChat?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task CreateAsync(GroupChat groupChat, CancellationToken cancellationToken = default);
    void Update(GroupChat groupChat);
    void Delete(GroupChat groupChat);
}