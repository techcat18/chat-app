using ChatApplication.DAL.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApplication.DAL.Data;

public class UnitOfWork: IUnitOfWork
{
    private bool _disposed;
    private readonly ChatDbContext _context;
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(
        ChatDbContext context, 
        IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public TRepository GetRepository<TRepository>()
    {
        var type = typeof(TRepository);
        
        if (_repositories.ContainsKey(type))
        {
            return (TRepository)_repositories[type];
        }

        var repositoryInstance = _serviceProvider.GetRequiredService<TRepository>();
        _repositories.Add(type, repositoryInstance);
        
        return repositoryInstance;
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