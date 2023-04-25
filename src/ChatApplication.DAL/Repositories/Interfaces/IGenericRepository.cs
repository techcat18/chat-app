using ChatApplication.DAL.Entities.Interfaces;

namespace ChatApplication.DAL.Repositories.Interfaces;

public interface IGenericRepository<TEntity> where TEntity: IBaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
}