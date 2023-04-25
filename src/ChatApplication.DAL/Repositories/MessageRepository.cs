using ChatApplication.DAL.Data;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.DAL.Repositories;

public class MessageRepository: IMessageRepository
{
    private readonly DbSet<Message> _messages;

    public MessageRepository(ChatDbContext context)
    {
        _messages = context.Set<Message>();
    }
    
    public async Task<IEnumerable<Message>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _messages.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Message>> GetByChatIdAsync(int chatId, CancellationToken cancellationToken = default)
    {
        return await _messages
            .Where(m => m.ChatId == chatId)
            .Include(m => m.Sender)
            .OrderByDescending(m => m.DateSent)
            .ToListAsync(cancellationToken);
    }

    public async Task<Message?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _messages
            .Include(m => m.Sender)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task CreateAsync(Message message, CancellationToken cancellationToken = default)
    {
        await _messages.AddAsync(message, cancellationToken);
    }
}