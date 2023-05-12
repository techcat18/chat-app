using ChatApplication.BLL.Tests.DummyDataGenerators.Chat;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Models.Chat;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ChatApplication.BLL.Tests.Services.ChatService.GetByIdAsync;

[TestFixture]
public class GivenValidInput: ChatServiceBaseSetup
{
    private Chat _chatEntity;
    private ChatModel _expected;
    private ChatModel _actual;

    [OneTimeSetUp]
    public async Task Init()
    {
        _chatEntity = new ChatEntityGenerator().Generate();
        _expected = new ChatModelGenerator().Generate();
        
        _mockUnitOfWork
            .Setup(uow => uow.GetRepository<IChatRepository>().GetByIdAsync(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(_chatEntity);

        _mockMapper
            .Setup(mapper => mapper.Map<ChatModel>(It.IsAny<Chat>()))
            .Returns(_expected);

        _actual = await _sut.GetByIdAsync(_chatEntity.Id);
    }

    [Test]
    public void ThenChatRepositoryGetByIdAsyncIsCalledOnce()
    {
        _mockUnitOfWork
            .Verify(uow => uow.GetRepository<IChatRepository>().GetByIdAsync(_chatEntity.Id, CancellationToken.None), Times.Once);
    }

    [Test]
    public void ThenActualChatMatchesExpected()
    {
        _actual.Should().BeEquivalentTo(_expected);
    }
}