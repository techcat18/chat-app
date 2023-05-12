using ChatApplication.BLL.Tests.DummyDataGenerators.Chat;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace ChatApplication.BLL.Tests.Services.ChatService.DeleteAsync;

[TestFixture]
public class GivenValidInput: ChatServiceBaseSetup
{
    private Chat _chatEntity;

    [OneTimeSetUp]
    public async Task Init()
    {
        _chatEntity = new ChatEntityGenerator().Generate();
        
        _mockUnitOfWork
            .Setup(uow => uow.GetRepository<IChatRepository>().GetByIdAsync(_chatEntity.Id, CancellationToken.None))
            .ReturnsAsync(_chatEntity);

        await _sut.DeleteAsync(_chatEntity.Id, CancellationToken.None);
    }

    [Test]
    public void ThenChatRepositoryGetByIdAsyncIsCalledOnce()
    {
        _mockUnitOfWork
            .Verify(uow => uow.GetRepository<IChatRepository>().GetByIdAsync(_chatEntity.Id, CancellationToken.None), Times.Once);
    }

    [Test]
    public void ThenChatRepositoryDeleteIsCalledOnce()
    {
        _mockUnitOfWork
            .Verify(uow => uow.GetRepository<IChatRepository>().Delete(_chatEntity), Times.Once);
    }

    [Test]
    public void ThenContextSaveChangesIsCalledOnce()
    {
        _mockUnitOfWork
            .Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
}