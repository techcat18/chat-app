using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Exceptions.NotFound;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ChatApplication.BLL.Tests.Services.ChatService.GetByIdAsync;

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
            await _sut.GetByIdAsync(_chatId);
        }
        catch (Exception e)
        {
            _exception = e;
        }
    }
    
    [Test]
    public void ThenChatRepositoryGetByIdAsyncIsCalledOnce()
    {
        _mockUnitOfWork
            .Verify(uow => uow.GetRepository<IChatRepository>().GetByIdAsync(_chatId, CancellationToken.None));
    }

    [Test]
    public void ThenChatNotFoundExceptionIsThrown()
    {
        _exception.Should().BeOfType<ChatNotFoundException>();
    }
}