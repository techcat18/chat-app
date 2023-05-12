using ChatApplication.BLL.Tests.DummyDataGenerators.Message;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Exceptions.NotFound;
using ChatApplication.Shared.Models.Message;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ChatApplication.BLL.Tests.Services.MessageService.CreateAsync;

[TestFixture]
public class GivenInvalidSenderId: MessageServiceBaseSetup
{
    private Message _messageEntity;
    private CreateMessageModel _messageToCreate;
    private Exception _exception;

    [OneTimeSetUp]
    public async Task Init()
    {
        _messageEntity = new MessageEntityGenerator().Generate();
        _messageToCreate = new CreateMessageModelGenerator().Generate();

        _mockUnitOfWork
            .Setup(uow => uow.GetRepository<IChatRepository>().GetByIdAsync(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(new Chat());

        _mockUnitOfWork
            .Setup(uow => uow.GetRepository<IUserRepository>().GetByIdAsync(It.IsAny<string>(), CancellationToken.None));

        try
        {
            await _sut.CreateAsync(_messageToCreate);
        }
        catch (Exception e)
        {
            _exception = e;
        }
    }

    [Test]
    public void ThenUserNotFoundExceptionIsThrown()
    {
        _exception.Should().BeOfType<UserNotFoundException>();
    }

    [Test]
    public void ThenMessageRepositoryCreateAsyncIsNeverCalled()
    {
        _mockUnitOfWork
            .Verify(uow =>
                    uow.GetRepository<IMessageRepository>().CreateAsync(It.IsAny<Message>(), CancellationToken.None),
                Times.Never);
    }

    [Test]
    public void ThenContextSaveChangesIsNeverCalled()
    {
        _mockUnitOfWork
            .Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Never);
    }
}