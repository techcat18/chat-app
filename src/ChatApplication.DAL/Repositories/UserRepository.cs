using ChatApplication.DAL.Data;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Entities.Views;
using ChatApplication.DAL.Extensions;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.DAL.Repositories;

public class UserRepository: IUserRepository
{
    private readonly DbSet<User> _users;
    private readonly ChatDbContext _context;

    public UserRepository(ChatDbContext context)
    {
        _context = context;
        _users = context.Users;
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _users
            .Include(u => u.UserChats)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserView>> GetByFilterAsync(
        UserFilterModel filterModel,
        CancellationToken cancellationToken = default)
    {
        return await _context.UserView.AsQueryable()
            .FilterBySearchString(filterModel.SearchString)
            .Sort(filterModel.OrderBy)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetByChatIdAsync(
        int chatId, 
        CancellationToken cancellationToken = default)
    {
        return await _users
            .Include(u => u.UserChats)
            .Where(u => u.UserChats.Any(uc => uc.ChatId == chatId))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllExceptByChatIdAsync(int chatId, CancellationToken cancellationToken = default)
    {
        return await _users
            .Include(u => u.UserChats)
            .Where(u => u.UserChats.All(uc => uc.ChatId != chatId))
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(
        UserFilterModel filterModel,
        CancellationToken cancellationToken = default)
    {
        return await _users
            .FilterBySearchString(filterModel.SearchString)
            .CountAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _users
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public void Update(User user)
    {
        _users.Update(user);
    }
}