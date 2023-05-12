using ChatApplication.BLL.Tests.DummyDataGenerators.Chat;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Models.Chat;
using Moq;
using NUnit.Framework;

namespace ChatApplication.BLL.Tests.Services.ChatService.UpdateAsync;

[TestFixture]
public class GivenValidInput: ChatServiceBaseSetup
{
    private Chat _chatEntity;
    private UpdateChatModel _updateChatModel;
    
    [OneTimeSetUp]
    public async Task Init()
    {
        _chatEntity = new ChatEntityGenerator().Generate();
        _updateChatModel = new UpdateChatModelGenerator().Generate();

        _mockUnitOfWork
            .Setup(uow => uow.GetRepository<IChatRepository>().GetByIdAsync(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(_chatEntity);

        await _sut.UpdateAsync(_updateChatModel);
    }

    [Test]
    public void ThenChatRepositoryGetByIdAsyncIsCalledOnce()
    {
        _mockUnitOfWork
            .Verify(uow => uow.GetRepository<IChatRepository>().GetByIdAsync(_chatEntity.Id, CancellationToken.None), Times.Once);
    }

    [Test]
    public void ThenChatRepositoryUpdateIsCalledOnce()
    {
        _mockUnitOfWork
            .Verify(uow => uow.GetRepository<IChatRepository>().Update(_chatEntity), Times.Once);
    }

    [Test]
    public void ThenContextSaveChangesAsyncIsCalledOnce()
    {
        _mockUnitOfWork
            .Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
}