using ChatApplication.DAL.Data;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Entities.Interfaces;
using ChatApplication.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.DAL.Repositories;

public class GenericRepository<TEntity>: IGenericRepository<TEntity> where TEntity: class, IBaseEntity
{
    private readonly ChatDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    protected GenericRepository(ChatDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }
}