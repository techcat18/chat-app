using ChatApplication.DAL.Data;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.DAL.Repositories;

public class ChatRepository: GenericRepository<Guid, Chat>, IChatRepository
{
    public ChatRepository(ChatDbContext context) : base(context)
    {
    }

    public async Task<Chat?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(gc => gc.Id == id, cancellationToken);
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
}