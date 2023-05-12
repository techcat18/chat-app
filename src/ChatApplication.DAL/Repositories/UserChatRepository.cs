using ChatApplication.DAL.Data;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.DAL.Repositories;

public class UserChatRepository: IUserChatRepository
{
    private readonly DbSet<UserChat> _userChats;

    public UserChatRepository(ChatDbContext context)
    {
        _userChats = context.Set<UserChat>();
    }

    public async Task CreateAsync(
        UserChat userChat, 
        CancellationToken cancellationToken = default)
    {
        await _userChats.AddAsync(userChat, cancellationToken);
    }

    public async Task CreateRangeAsync(
        IEnumerable<UserChat> userChats, 
        CancellationToken cancellationToken = default)
    {
        await _userChats.AddRangeAsync(userChats, cancellationToken);
    }
}