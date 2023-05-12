using ChatApplication.BLL.Tests.DummyDataGenerators.Chat;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Exceptions.NotFound;
using ChatApplication.Shared.Models.Chat;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ChatApplication.BLL.Tests.Services.ChatService.UpdateAsync;

[TestFixture]
public class GivenInvalidId: ChatServiceBaseSetup
{
    private UpdateChatModel _updateChatModel;
    private Exception _exception;
    private Func<Task<ChatModel?>> _func;

    [OneTimeSetUp]
    public async Task Init()
    {
        _updateChatModel = new UpdateChatModelGenerator().Generate();

        _mockUnitOfWork
            .Setup(uow => uow.GetRepository<IChatRepository>().GetByIdAsync(It.IsAny<int>(), CancellationToken.None));

        // Arrange
        try
        {
            await _sut.UpdateAsync(_updateChatModel);
        }
        catch (Exception e)
        {
            _exception = e;
        }
        
        _func = async () => await _sut.GetByIdAsync(_updateChatModel.Id);
    }

    // Assert
    [Test]
    public void ThenChatNotFoundExceptionIsThrown()
    {
        _exception.Should().BeOfType<ChatNotFoundException>();
        _func.Should().ThrowAsync<ChatNotFoundException>();
    }

    [Test]
    public void ThenChatRepositoryUpdateIsNeverCalled()
    {
        _mockUnitOfWork
            .Verify(uow => uow.GetRepository<IChatRepository>().Update(It.IsAny<Chat>()), Times.Never);
    }

    [Test]
    public void ThenContextSaveChangesIsNeverCalled()
    {
        _mockUnitOfWork
            .Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Never);
    }
}