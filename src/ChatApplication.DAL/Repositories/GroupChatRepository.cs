using ChatApplication.DAL.Data;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.DAL.Repositories;

public class GroupChatRepository: GenericRepository<Guid, GroupChat>, IGroupChatRepository
{
    public GroupChatRepository(ChatDbContext context) : base(context)
    {
    }

    public async Task<GroupChat?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(gc => gc.Id == id, cancellationToken);
    }

    public async Task CreateAsync(GroupChat groupChat, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(groupChat, cancellationToken);
    }

    public void Update(GroupChat groupChat)
    {
        _dbSet.Update(groupChat);
    }

    public void Delete(GroupChat groupChat)
    {
        _dbSet.Remove(groupChat);
    }
}