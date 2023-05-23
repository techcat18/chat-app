using ChatApplication.BLL.Services;
using ChatApplication.DAL.Data;
using ChatApplication.IntegrationTests.Utility;
using ChatApplication.Shared.Exceptions.BadRequest;
using ChatApplication.Shared.Exceptions.NotFound;
using ChatApplication.Shared.Models.User;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace ChatApplication.IntegrationTests.Services;

[TestFixture]
public class UserServiceTests
{
    private UserService _userService;
    private ChatDbContext _context;

    [OneTimeSetUp]
    public async Task Init()
    {
        var dbOptions = DbHelper.GetDbContextOptions();
        _context = await DbHelper.CreateChatDbContextAsync(dbOptions);

        var serviceProvider = ServiceHelper.RegisterServices(_context);
        var unitOfWork = ServiceHelper.CreateUnitOfWork(_context, serviceProvider);
        var mapper = ServiceHelper.CreateMapper();

        _userService = new UserService(
            unitOfWork,
            mapper,
            null); //TODO: fix later
    }
    
    [SetUp]
    public async Task TestSetup()
    {
        await _context.ClearData();
        await _context.SeedChatTypesAsync();
        await _context.SeedChats();
        await _context.SeedUsers();
        await _context.SeedUserChats();
    }

    [Test]
    public async Task GetAllAsync_ReturnsEntities()
    {
        var expected = await _context.Users.ToListAsync();

        var actual = await _userService.GetAllAsync();

        actual.Should().BeEquivalentTo(expected, opt => opt
            .ExcludingMissingMembers());
    }

    [Test]
    public async Task GetAllByChatIdAsync_GivenValidChatId_ReturnsEntities()
    {
        var chatId = (await _context.Chats.FirstAsync()).Id;
        var expected = await _context.Users
            .Include(u => u.UserChats)
            .Where(u => u.UserChats.Any(uc => uc.ChatId == chatId))
            .ToListAsync();

        var actual = await _userService.GetAllByChatIdAsync(chatId);

        actual.Should().BeEquivalentTo(expected, opt => opt
            .ExcludingMissingMembers());
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-100)]
    public async Task GetAllByChatIdAsync_GivenInvalidChatId_ThrowsChatNotFoundException(int chatId)
    {
        var func = async () => await _userService.GetAllByChatIdAsync(chatId);

        await func.Should().ThrowAsync<ChatNotFoundException>();
    }

    [Test]
    public async Task GetAllExceptByChatId_GivenValidChatId_ReturnsEntities()
    {
        var chatId = (await _context.Chats.FirstAsync()).Id;
        var expected = await _context.Users
            .Include(u => u.UserChats)
            .Where(u => u.UserChats.All(uc => uc.ChatId != chatId))
            .ToListAsync();

        var actual = await _userService.GetAllExceptByChatIdAsync(chatId);

        actual.Should().BeEquivalentTo(expected, opt => opt
            .ExcludingMissingMembers());
    }
    
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-100)]
    public async Task GetAllExceptByChatId_GivenInValidChatId_ThrowsChatNotFoundException(int chatId)
    {
        var func = async () => await _userService.GetAllExceptByChatIdAsync(chatId);

        await func.Should().ThrowAsync<ChatNotFoundException>();
    }
    
    [Test]
    public async Task GetByIdAsync_GivenValidId_ReturnsSingleEntity()
    {
        var userId = (await _context.Users.FirstAsync()).Id;

        var actual = await _userService.GetByIdAsync(userId);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(userId);
    }

    [TestCase("invalid")]
    [TestCase("doesnt_exist")]
    [TestCase("fake_id")]
    public async Task GetByIdAsync_GivenInvalidId_ThrowsUserNotFoundException(string id)
    {
        var func = async () => await _userService.GetByIdAsync(id);

        await func.Should().ThrowAsync<UserNotFoundException>();
    }

    [Test]
    public async Task UpdateAsync_GivenValidEntity_UpdatesEntityInDatabase()
    {
        var userId = (await _context.Users.FirstAsync()).Id;
        var updateUserModel = new UpdateUserModel
        {
            Id = userId,
            Age = 10,
            Email = "updatedEmail@gmail.com",
            FirstName = "Updated First Name",
            LastName = "Updated Last Name"
        };

        await _userService.UpdateAsync(updateUserModel);

        var updatedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        updatedUser.Should().BeEquivalentTo(updateUserModel, opt => opt
            .ExcludingMissingMembers());
    }

    [Test]
    public async Task UpdateAsync_GivenInvalidEmail_ThrowsAuthException()
    {
        var userId = (await _context.Users.FirstAsync()).Id;
        var email = (await _context.Users.FirstOrDefaultAsync(u => u.Id != userId))?.Email;
        var updateUserModel = new UpdateUserModel
        {
            Id = userId,
            Age = 10,
            Email = email,
            FirstName = "Updated First Name",
            LastName = "Updated Last Name"
        };

        var func = async () => await _userService.UpdateAsync(updateUserModel);

        await func.Should().ThrowAsync<UserAlreadyExistsException>();
    }
}