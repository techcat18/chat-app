using ChatApplication.DAL.Repositories.Interfaces;

namespace ChatApplication.DAL.Data.Interfaces;

public interface IUnitOfWork: IDisposable
{
    public IGroupChatRepository GroupChatRepository { get; }
}