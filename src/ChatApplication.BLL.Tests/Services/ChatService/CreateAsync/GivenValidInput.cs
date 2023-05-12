using ChatApplication.BLL.Tests.DummyDataGenerators.Chat;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Models.Chat;
using Moq;
using NUnit.Framework;

namespace ChatApplication.BLL.Tests.Services.ChatService.CreateAsync;

[TestFixture]
public class GivenValidInput: ChatServiceBaseSetup
{
    private Chat _chatEntity;
    private ChatModel _chatModel;
    private CreateChatModel _createChatModel;

    [OneTimeSetUp]
    public async Task Init()
    {
        _chatEntity = new ChatEntityGenerator().Generate();
        _createChatModel = new CreateChatModel { ChatTypeId = _chatEntity.ChatTypeId, Name = _chatEntity.Name };
        
        _mockUnitOfWork
            .Setup(uow =>
                uow.GetRepository<IChatRepository>().GetByNameAsync(It.IsAny<string>(), CancellationToken.None));

        _mockUnitOfWork
            .Setup(uow => uow.GetRepository<IChatRepository>().CreateAsync(It.IsAny<Chat>(), CancellationToken.None));

        _mockMapper
            .Setup(mapper => mapper.Map<Chat>(It.IsAny<CreateChatModel>()))
            .Returns(_chatEntity);

        _mockMapper
            .Setup(mapper => mapper.Map<ChatModel>(It.IsAny<Chat>()))
            .Returns(_chatModel);

        await _sut.CreateAsync(_createChatModel);
    }

    [Test]
    public void ThenChatRepositoryGetByNameIsCalledOnce()
    {
        _mockUnitOfWork
            .Verify(uow =>
                uow.GetRepository<IChatRepository>().GetByNameAsync(_createChatModel.Name, CancellationToken.None));
    }

    [Test]
    public void ThenChatRepositoryCreateAsyncIsCalledOnce()
    {
        _mockUnitOfWork
            .Verify(uow => uow.GetRepository<IChatRepository>().CreateAsync(_chatEntity, CancellationToken.None),
                Times.Once);
    }
    
    [Test]
    public void ThenContextSaveChangesIsCalledOnce()
    {
        _mockUnitOfWork
            .Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
}