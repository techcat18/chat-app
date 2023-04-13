using ChatApplication.DAL.Repositories.Interfaces;

namespace ChatApplication.DAL.Data.Interfaces;

public interface IUnitOfWork: IDisposable
{
    public IChatRepository ChatRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}