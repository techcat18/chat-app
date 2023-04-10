using ChatApplication.DAL.Data.Interfaces;
using ChatApplication.DAL.Repositories;
using ChatApplication.DAL.Repositories.Interfaces;

namespace ChatApplication.DAL.Data;

public class UnitOfWork: IUnitOfWork
{
    private bool _disposed;
    private readonly ChatDbContext _context;
    private IGroupChatRepository? _groupChatRepository;

    public UnitOfWork(ChatDbContext context)
    {
        _context = context;
    }

    public IGroupChatRepository GroupChatRepository
    {
        get { return _groupChatRepository ??= new GroupChatRepository(_context); }
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