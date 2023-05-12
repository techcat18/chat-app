using ChatApplication.BLL.Tests.DummyDataGenerators.Chat;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Models.Chat;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ChatApplication.BLL.Tests.Services.ChatService.GetAllAsync;

[TestFixture]
public class GivenValidInput: ChatServiceBaseSetup
{
    private IEnumerable<Chat> _chatEntities;
    private IEnumerable<ChatModel> _expected;
    private IEnumerable<ChatModel> _actual;

    [OneTimeSetUp]
    public async Task Init()
    {
        _chatEntities = new ChatEntityGenerator().Generate(10);
        _expected = new ChatModelGenerator().Generate(10);

        _mockUnitOfWork
            .Setup(uow => uow.GetRepository<IChatRepository>().GetAllAsync(CancellationToken.None))
            .ReturnsAsync(_chatEntities);

        _mockMapper
            .Setup(mapper => mapper.Map<IEnumerable<ChatModel>>(It.IsAny<IEnumerable<Chat>>()))
            .Returns(_expected);

        _actual = await _sut.GetAllAsync();
    }

    [Test]
    public void ThenChatRepositoryGetAllAsyncIsCalledOnce()
    {
        _mockUnitOfWork.VerifyAll();
    }

    [Test]
    public void ThenActualChatsMatchExpected()
    {
        _actual.Should().Equal(_expected);
    }
}