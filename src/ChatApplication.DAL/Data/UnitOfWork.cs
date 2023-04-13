using ChatApplication.DAL.Data.Interfaces;
using ChatApplication.DAL.Repositories;
using ChatApplication.DAL.Repositories.Interfaces;

namespace ChatApplication.DAL.Data;

public class UnitOfWork: IUnitOfWork
{
    private bool _disposed;
    private readonly ChatDbContext _context;
    private IChatRepository? _groupChatRepository;

    public UnitOfWork(ChatDbContext context)
    {
        _context = context;
    }

    public IChatRepository ChatRepository
    {
        get { return _groupChatRepository ??= new ChatRepository(_context); }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}