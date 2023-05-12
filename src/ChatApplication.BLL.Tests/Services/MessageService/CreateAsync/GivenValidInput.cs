using ChatApplication.BLL.Tests.DummyDataGenerators.Message;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Models.Message;
using Moq;
using NUnit.Framework;

namespace ChatApplication.BLL.Tests.Services.MessageService.CreateAsync;

[TestFixture]
public class GivenValidInput: MessageServiceBaseSetup
{
    private Message _messageEntity;
    private CreateMessageModel _messageToCreate;
    private MessageModel _expected;
    
    [OneTimeSetUp]
    public async Task Init()
    {
        _messageEntity = new MessageEntityGenerator().Generate();
        _messageToCreate = new CreateMessageModelGenerator().Generate();
        _expected = new MessageModelGenerator().Generate();

        _mockUnitOfWork
            .Setup(uow => uow.GetRepository<IMessageRepository>().CreateAsync(It.IsAny<Message>(), CancellationToken.None));

        _mockUnitOfWork
            .Setup(uow => uow.GetRepository<IChatRepository>().GetByIdAsync(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(new Chat());

        _mockUnitOfWork
            .Setup(uow => uow.GetRepository<IUserRepository>().GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new User());

        _mockMapper
            .Setup(mapper => mapper.Map<Message>(It.IsAny<CreateMessageModel>()))
            .Returns(_messageEntity);

        _mockMapper
            .Setup(mapper => mapper.Map<MessageModel>(_messageEntity));

        await _sut.CreateAsync(_messageToCreate);
    }

    [Test]
    public void ThenMessageRepositoryCreateAsyncIsCalledOnce()
    {
        _mockUnitOfWork
            .Verify(uow => uow.GetRepository<IMessageRepository>().CreateAsync(_messageEntity, CancellationToken.None), Times.Once);
    }

    [Test]
    public void ThenContextSaveChangesAsyncIsCalledOnce()
    {
        _mockUnitOfWork
            .Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
}