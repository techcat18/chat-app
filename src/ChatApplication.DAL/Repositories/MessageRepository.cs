using ChatApplication.DAL.Data;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Entities.Functions;
using ChatApplication.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.DAL.Repositories;

public class MessageRepository: IMessageRepository
{
    private readonly DbSet<Message> _messages;
    private readonly ChatDbContext _context;

    public MessageRepository(ChatDbContext context)
    {
        _context = context;
        _messages = context.Set<Message>();
    }
    
    public async Task<IEnumerable<Message>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _messages.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MessageFunction>> GetByChatIdAsync(int chatId, CancellationToken cancellationToken = default)
    {
        return await _context.MessagesByChatIdFunc(chatId)
            .OrderByDescending(m => m.DateSent)
            .ToListAsync(cancellationToken);
    }

    public async Task<MessageFunction?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.MessageByIdFunc(id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task CreateAsync(Message message, CancellationToken cancellationToken = default)
    {
        await _messages.AddAsync(message, cancellationToken);
    }
}