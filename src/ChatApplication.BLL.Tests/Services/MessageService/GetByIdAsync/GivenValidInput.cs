using ChatApplication.BLL.Tests.DummyDataGenerators.Message;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Entities.Functions;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Models.Message;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ChatApplication.BLL.Tests.Services.MessageService.GetByIdAsync;

[TestFixture]
public class GivenValidInput: MessageServiceBaseSetup
{
    private MessageFunction _messageEntity;
    private MessageModel _expected;
    private MessageModel _actual;
    private readonly int _id;
    
    [OneTimeSetUp]
    public async Task Init()
    {
        _messageEntity = new MessageFunctionGenerator().Generate();
        _expected = new MessageModelGenerator().Generate();
        
        _mockUnitOfWork
            .Setup(uow => uow.GetRepository<IMessageRepository>().GetByIdAsync(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(_messageEntity);

        _mockMapper
            .Setup(mapper => mapper.Map<MessageModel>(It.IsAny<MessageFunction>()))
            .Returns(_expected);

        _actual = await _sut.GetByIdAsync(_id);
    }
    
    [Test]
    public void ThenMessageRepositoryGetByChatIdAsyncIsCalledOnce()
    {
        _mockUnitOfWork
            .Verify(uow => uow.GetRepository<IMessageRepository>().GetByIdAsync(_id, CancellationToken.None),
                Times.Once);
    }

    [Test]
    public void ThenActualMatchExpected()
    {
        _actual.Should().BeEquivalentTo(_expected);
    }
}