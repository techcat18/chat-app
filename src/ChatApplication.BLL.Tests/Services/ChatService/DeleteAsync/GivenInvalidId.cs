using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Exceptions.NotFound;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ChatApplication.BLL.Tests.Services.ChatService.DeleteAsync;

[TestFixture]
public class GivenInvalidId: ChatServiceBaseSetup
{
    private readonly int _chatId = 0;
    private Exception _exception;
    
    [OneTimeSetUp]
    public async Task Init()
    {
        _mockUnitOfWork
            .Setup(uow => uow.GetRepository<IChatRepository>().GetByIdAsync(It.IsAny<int>(), CancellationToken.None));
        
        try
        {
            await _sut.DeleteAsync(_chatId);
        }
        catch (Exception e)
        {
            _exception = e;
        }
    }

    [Test]
    public void ThenChatNotFoundExceptionIsThrown()
    {
        _exception.Should().BeOfType<ChatNotFoundException>();
    }

    [Test]
    public void ThenChatRepositoryGetByIdAsyncIsCalledOnce()
    {
        _mockUnitOfWork
            .Verify(uow => uow.GetRepository<IChatRepository>().GetByIdAsync(_chatId, CancellationToken.None), Times.Once);
    }

    [Test]
    public void ThenChatRepositoryDeleteIsNeverCalled()
    {
        _mockUnitOfWork
            .Verify(uow => uow.GetRepository<IChatRepository>().Delete(It.IsAny<Chat>()), Times.Never);
    }

    [Test]
    public void ThenContextSaveChangesAsyncIsNeverCalled()
    {
        _mockUnitOfWork.VerifyNoOtherCalls();
    }
}