using ChatApplication.DAL.Data;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Entities.Functions;
using ChatApplication.DAL.Entities.Views;
using ChatApplication.DAL.Extensions;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.DAL.Repositories;

public class ChatRepository: GenericRepository<Chat>, IChatRepository
{
    public ChatRepository(ChatDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ChatView>> GetAllByFilterAsync(
        ChatFilterModel filterModel,
        CancellationToken cancellationToken = default)
    {
        return await _context.ChatView.AsQueryable()
            .FilterByName(filterModel.SearchString)
            .Sort(filterModel.SortingOption)
            .Paginate(filterModel.Page, filterModel.Count)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ChatFunction>> GetAllByUserIdAsync(
        string userId,
        ChatFilterModel filterModel,
        CancellationToken cancellationToken = default)
    {
        return await _context.ChatsByUserIdFunc(userId)
            .FilterByName(filterModel.SearchString)
            .Sort(filterModel.SortingOption)
            .Paginate(filterModel.Page, filterModel.Count)
            .ToListAsync(cancellationToken);
    }

    public async Task<Chat?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Chat?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
    }

    public async Task CreateAsync(Chat chat, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(chat, cancellationToken);
    }

    public void Update(Chat chat)
    {
        _dbSet.Update(chat);
    }

    public void Delete(Chat chat)
    {
        _dbSet.Remove(chat);
    }

    public async Task<int> GetTotalCountAsync(
        ChatFilterModel filterModel,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FilterByName(filterModel.SearchString)
            .Sort(filterModel.SortingOption)
            .CountAsync(cancellationToken);
    }
}