using ChatApplication.BLL.Services;
using ChatApplication.DAL.Data;
using ChatApplication.IntegrationTests.Utility;
using ChatApplication.Shared.Exceptions.BadRequest;
using ChatApplication.Shared.Exceptions.NotFound;
using ChatApplication.Shared.Models.Chat;
using FluentAssertions;
using NUnit.Framework;

namespace ChatApplication.IntegrationTests.Services;

[TestFixture]
public class ChatServiceTests
{
    private ChatService _chatService;
    private ChatDbContext _context;

    [OneTimeSetUp]
    public async Task Init()
    {
        var dbOptions = DbHelper.GetDbContextOptions();
        _context = await DbHelper.CreateChatDbContextAsync(dbOptions);

        var serviceProvider = ServiceHelper.RegisterServices(_context);
        var unitOfWork = ServiceHelper.CreateUnitOfWork(_context, serviceProvider);
        var mapper = ServiceHelper.CreateMapper();
        
        _chatService = new ChatService(unitOfWork, mapper);
    }

    [SetUp]
    public async Task TestSetup()
    {
        await _context.ClearData();
        await _context.SeedChatTypesAsync();
        await _context.SeedChats();
    }

    [Test]
    public async Task GetAllAsync_ReturnsEntities()
    {
        var actual = await _chatService.GetAllAsync();

        actual.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task GetByIdAsync_ReturnsSingleEntity()
    {
        var chat = _context.Chats.First();

        var actual = await _chatService.GetByIdAsync(chat.Id);

        actual.Should().BeEquivalentTo(chat, opt => opt
            .ExcludingMissingMembers());
    }

    [Test]
    public async Task CreateAsync_ValidEntity_AddsEntityToDatabase()
    {
        var createChatModel = new CreateChatModel { ChatTypeId = _context.ChatTypes.First().Id, Name = "New Chat" };
        var beforeCount = _context.Chats.Count();

        await _chatService.CreateAsync(createChatModel);

        _context.Chats.FirstOrDefault(c => c.Name == createChatModel.Name).Should().NotBeNull();
        _context.Chats.Count().Should().Be(beforeCount + 1);
    }

    [Test]
    public async Task CreateAsync_NameAlreadyExists_ThrowsChatAlreadyExistsException()
    {
        var existingChatName = _context.Chats.First().Name;
        var createChatModel = new CreateChatModel { ChatTypeId = _context.ChatTypes.First().Id, Name = existingChatName };
        var beforeCount = _context.Chats.Count();
        
        var func = async () => await _chatService.CreateAsync(createChatModel);

        await func.Should().ThrowAsync<ChatAlreadyExistsException>();
        _context.Chats.Count().Should().Be(beforeCount);
    }

    [Test]
    public async Task UpdateAsync_ValidEntity_UpdatesEntityInDatabase()
    {
        var chat = _context.Chats.First();
        var updateChatModel = new UpdateChatModel { ChatTypeId = chat.ChatTypeId, Id = chat.Id, Name = "Updated Name" };

        await _chatService.UpdateAsync(updateChatModel);

        updateChatModel.Should().BeEquivalentTo(chat, opt => opt
            .ExcludingMissingMembers());
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-100)]
    public async Task UpdateAsync_InvalidId_ThrowsChatNotFoundException(int chatId)
    {
        var updateChatModel = new UpdateChatModel { Id = chatId, Name = "Updated Name" };

        var func = async () => await _chatService.UpdateAsync(updateChatModel);

        await func.Should().ThrowAsync<ChatNotFoundException>();
    }

    [Test]
    public async Task DeleteAsync_ValidId_RemovesEntityFromDatabase()
    {
        var chat = _context.Chats.First();
        var beforeCount = _context.Chats.Count();

        await _chatService.DeleteAsync(chat.Id);

        _context.Chats.Count().Should().Be(beforeCount - 1);
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-100)]
    public async Task DeleteAsync_InvalidId_ThrowsChatNotFoundException(int chatId)
    {
        var beforeCount = _context.Chats.Count();

        var func = async () => await _chatService.DeleteAsync(chatId);

        await func.Should().ThrowAsync<ChatNotFoundException>();
        _context.Chats.Count().Should().Be(beforeCount);
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}