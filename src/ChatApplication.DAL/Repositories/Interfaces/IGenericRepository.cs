using ChatApplication.DAL.Entities;

namespace ChatApplication.DAL.Repositories.Interfaces;

public interface IGenericRepository<TId, TEntity> where TEntity: BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
}