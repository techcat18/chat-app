using ChatApplication.DAL.Data;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Extensions;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.DAL.Repositories;

public class UserRepository: IUserRepository
{
    private readonly DbSet<User> _users;

    public UserRepository(ChatDbContext context)
    {
        _users = context.Users;
    }

    public async Task<IEnumerable<User>> GetByFilterAsync(
        UserFilterModel filterModel,
        CancellationToken cancellationToken = default)
    {
        return await _users
            .FilterByEmail(filterModel.Email)
            .FilterByUserName(filterModel.UserName)
            .Paginate(filterModel.Page, filterModel.Count)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(
        UserFilterModel filterModel,
        CancellationToken cancellationToken = default)
    {
        return await _users
            .FilterByEmail(filterModel.Email)
            .FilterByUserName(filterModel.UserName)
            .CountAsync(cancellationToken);
    }
}