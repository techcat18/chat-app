namespace ChatApplication.DAL.Data.Interfaces;

public interface IUnitOfWork: IDisposable
{
    TRepository GetRepository<TRepository>();
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}