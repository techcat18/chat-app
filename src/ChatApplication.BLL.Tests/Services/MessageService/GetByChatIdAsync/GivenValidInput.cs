using ChatApplication.BLL.Tests.DummyDataGenerators.Message;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Entities.Functions;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Models.Message;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ChatApplication.BLL.Tests.Services.MessageService.GetByChatIdAsync;

[TestFixture]
public class GivenValidInput: MessageServiceBaseSetup
{
    private IEnumerable<MessageFunction> _messageEntities;
    private IEnumerable<MessageModel> _expected;
    private IEnumerable<MessageModel> _actual;
    private readonly int _chatId = 1;

    [OneTimeSetUp]
    public async Task Init()
    {
        _messageEntities = new MessageFunctionGenerator().Generate(10);
        _expected = new MessageModelGenerator().Generate(10);
        
        _mockUnitOfWork
            .Setup(uow =>
                uow.GetRepository<IMessageRepository>().GetByChatIdAsync(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(_messageEntities);

        _mockUnitOfWork
            .Setup(uow => uow.GetRepository<IChatRepository>().GetByIdAsync(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(new Chat());

        _mockMapper
            .Setup(mapper => mapper.Map<IEnumerable<MessageModel>>(It.IsAny<IEnumerable<MessageFunction>>()))
            .Returns(_expected);

        _actual = await _sut.GetByChatIdAsync(_chatId);
    }

    [Test]
    public void ThenMessageRepositoryGetByChatIdAsyncIsCalledOnce()
    {
        _mockUnitOfWork
            .Verify(uow => uow.GetRepository<IMessageRepository>().GetByChatIdAsync(_chatId, CancellationToken.None),
                Times.Once);
    }

    [Test]
    public void ThenActualMatchExpected()
    {
        _actual.Should().Equal(_expected);
    }
}