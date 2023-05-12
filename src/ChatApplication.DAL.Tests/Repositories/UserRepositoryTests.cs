using ChatApplication.DAL.Data;
using ChatApplication.DAL.Repositories;
using ChatApplication.Shared.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace ChatApplication.DAL.Tests.Repositories;

[TestFixture]
public class UserRepositoryTests
{
    [Test]
    public async Task GetAllAsync_ReturnsEntities()
    {
        var context = new ChatDbContext(await UnitTestHelper.GetInMemoryDbOptionsAsync());
        var repository = new UserRepository(context);
        var expected = context.Users.Include(u => u.UserChats).ToList();

        var actual = await repository.GetAllAsync();

        actual.Should().Equal(expected);
    }

    [Test]
    public async Task GetByFilterAsync_ReturnsEntities()
    {
        var context = new ChatDbContext(await UnitTestHelper.GetInMemoryDbOptionsAsync());
        var repository = new UserRepository(context);
        var expected = context.UserView.ToList();

        var actual = await repository.GetByFilterAsync(new UserFilterModel());

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetTotalCountAsync_ReturnsEntitiesCount()
    {
        var context = new ChatDbContext(await UnitTestHelper.GetInMemoryDbOptionsAsync());
        var repository = new UserRepository(context);
        var beforeCount = context.Users.Count();

        var actual = await repository.GetTotalCountAsync(new UserFilterModel());

        actual.Should().Be(beforeCount);
    }

    [Test]
    public async Task GetByIdAsync_ReturnsSingleEntity()
    {
        var context = new ChatDbContext(await UnitTestHelper.GetInMemoryDbOptionsAsync());
        var repository = new UserRepository(context);
        var expected = context.Users.First();

        var actual = await repository.GetByIdAsync(expected.Id);

        actual.Should().BeEquivalentTo(expected);
    }

    [TestCase(1)]
    [TestCase(2)]
    public async Task GetAllExceptByChatIdAsync_ReturnsEntities(int chatId)
    {
        var context = new ChatDbContext(await UnitTestHelper.GetInMemoryDbOptionsAsync());
        var repository = new UserRepository(context);
        var expected = context.Users
            .Include(u => u.UserChats)
            .Where(u => u.UserChats.All(uc => uc.ChatId != chatId));

        var actual = await repository.GetAllExceptByChatIdAsync(chatId);

        actual.Should().BeEquivalentTo(expected);
    }

    [TestCase(1)]
    [TestCase(2)]
    public async Task GetByChatIdAsync_ReturnsEntitiesByChatId(int chatId)
    {
        var context = new ChatDbContext(await UnitTestHelper.GetInMemoryDbOptionsAsync());
        var repository = new UserRepository(context);
        var expected = context.Users
            .Include(u => u.UserChats)
            .Where(u => u.UserChats.Any(uc => uc.ChatId == chatId));

        var actual = await repository.GetByChatIdAsync(chatId);

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task Update_UpdatesEntityInDatabase()
    {
        var context = new ChatDbContext(await UnitTestHelper.GetInMemoryDbOptionsAsync());
        var repository = new UserRepository(context);
        var entity = context.Users.First();
        entity.FirstName = "updated first name";

        repository.Update(entity);
        await context.SaveChangesAsync();

        context.Users.First().FirstName.Should().Be("updated first name");
    }
}