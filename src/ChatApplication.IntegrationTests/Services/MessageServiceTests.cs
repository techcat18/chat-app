using ChatApplication.BLL.Services;
using ChatApplication.DAL.Data;
using ChatApplication.IntegrationTests.Utility;
using ChatApplication.Shared.Exceptions.NotFound;
using ChatApplication.Shared.Models.Message;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace ChatApplication.IntegrationTests.Services;

[TestFixture]
public class MessageServiceTests
{
    private MessageService _messageService;
    private ChatDbContext _context;

    [OneTimeSetUp]
    public async Task Init()
    {
        var dbOptions = DbHelper.GetDbContextOptions();
        _context = await DbHelper.CreateChatDbContextAsync(dbOptions);

        var serviceProvider = ServiceHelper.RegisterServices(_context);
        var unitOfWork = ServiceHelper.CreateUnitOfWork(_context, serviceProvider);
        var mapper = ServiceHelper.CreateMapper();

        _messageService = new MessageService(unitOfWork, mapper);
    }

    [SetUp]
    public async Task TestSetup()
    {
        await _context.ClearData();
        await _context.SeedChatTypesAsync();
        await _context.SeedChats();
        await _context.SeedUsers();
        await _context.SeedUserChats();
        await _context.SeedMessages();
    }

    [Test]
    public async Task GetByIdAsync_GivenValidId_ReturnsSingleEntity()
    {
        var expected = _context.Messages.First();

        var actual = await _messageService.GetByIdAsync(expected.Id);

        actual.Id.Should().Be(expected.Id);
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-100)]
    public async Task GetByIdAsync_GivenInvalidId_ThrowsMessageNotFoundException(int id)
    {
        var func = async () => await _messageService.GetByIdAsync(id);

        await func.Should().ThrowAsync<MessageNotFoundException>();
    }
    
    [Test]
    public async Task GetByChatIdAsync_GivenValidId_ReturnsEntities()
    {
        var chatId = (await _context.Chats.FirstAsync()).Id;
        var expected = await _context.Messages.Where(m => m.ChatId == chatId).ToListAsync();

        var actual = await _messageService.GetByChatIdAsync(chatId);

        actual.Should().BeEquivalentTo(expected, opt => opt
            .ExcludingMissingMembers());
    }
    
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-100)]
    public async Task GetByChatIdAsync_GivenInValidId_ReturnsEntities(int chatId)
    {
        var func = async () => await _messageService.GetByChatIdAsync(chatId);

        await func.Should().ThrowAsync<ChatNotFoundException>();
    }

    [Test]
    public async Task CreateAsync_GivenValidEntity_AddsEntityToDatabase()
    {
        var chatId = (await _context.Chats.FirstAsync()).Id;
        var senderId = (await _context.Users.FirstAsync()).Id;
        var beforeCount = await _context.Messages.CountAsync();
        var createMessageModel = new CreateMessageModel { ChatId = chatId, SenderId = senderId, Content = "New message"};

        var createdMessage = await _messageService.CreateAsync(createMessageModel);

        (await _context.Messages.CountAsync()).Should().Be(beforeCount + 1);
        (await _context.Messages.FirstOrDefaultAsync(m => m.Id == createdMessage.Id)).Should().NotBeNull();
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-100)]
    public async Task CreateAsync_GivenInvalidChatId_ThrowsChatNotFoundException(int chatId)
    {
        var senderId = (await _context.Users.FirstAsync()).Id;
        var beforeCount = await _context.Messages.CountAsync();
        var createMessageModel = new CreateMessageModel { ChatId = chatId, SenderId = senderId, Content = "New message"};

        var func = async () => await _messageService.CreateAsync(createMessageModel);

        await func.Should().ThrowAsync<ChatNotFoundException>();
        (await _context.Messages.CountAsync()).Should().Be(beforeCount);
    }

    [TestCase("invalid")]
    [TestCase("doesnt_exist")]
    [TestCase("fake_id")]
    public async Task CreateAsync_GivenInvalidSenderId_ThrowsUserNotFoundException(string senderId)
    {
        var chatId = (await _context.Chats.FirstAsync()).Id;
        var beforeCount = await _context.Messages.CountAsync();
        var createMessageModel = new CreateMessageModel { ChatId = chatId, SenderId = senderId, Content = "New message"};

        var func = async () => await _messageService.CreateAsync(createMessageModel);

        await func.Should().ThrowAsync<UserNotFoundException>();
        (await _context.Messages.CountAsync()).Should().Be(beforeCount);
    }
}