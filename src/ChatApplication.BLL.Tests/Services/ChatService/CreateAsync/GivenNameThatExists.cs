using ChatApplication.BLL.Tests.DummyDataGenerators.Chat;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Exceptions.BadRequest;
using ChatApplication.Shared.Models.Chat;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ChatApplication.BLL.Tests.Services.ChatService.CreateAsync;

[TestFixture]
public class GivenNameThatExists: ChatServiceBaseSetup
{
    private Chat _chatEntity;
    private ChatModel _chatModel;
    private CreateChatModel _createChatModel;
    private Exception _exception;

    [OneTimeSetUp]
    public async Task Init()
    {
        _chatEntity = new ChatEntityGenerator().Generate();
        _createChatModel = new CreateChatModel { ChatTypeId = _chatEntity.ChatTypeId, Name = _chatEntity.Name };

        _mockUnitOfWork
            .Setup(uow =>
                uow.GetRepository<IChatRepository>().GetByNameAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(_chatEntity);

        try
        {
            await _sut.CreateAsync(_createChatModel);
        }
        catch (Exception e)
        {
            _exception = e;
        }
    }

    [Test]
    public void ThenChatAlreadyExistsExceptionIsThrown()
    {
        _exception.Should().BeOfType<ChatAlreadyExistsException>();
    }

    [Test]
    public void ThenChatRepositoryCreateAsyncIsNeverCalled()
    {
        _mockUnitOfWork
            .Verify(uow => uow.GetRepository<IChatRepository>().CreateAsync(It.IsAny<Chat>(), CancellationToken.None), Times.Never);
    }

    [Test]
    public void ThenContextSaveChangesAsyncIsNeverCalled()
    {
        _mockUnitOfWork
            .Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Never);
    }
}