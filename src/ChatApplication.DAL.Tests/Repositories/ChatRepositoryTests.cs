using ChatApplication.DAL.Data;
using ChatApplication.DAL.Repositories;
using ChatApplication.DAL.Tests.Builders.Chat;
using ChatApplication.Shared.Models;
using FluentAssertions;
using NUnit.Framework;

namespace ChatApplication.DAL.Tests.Repositories;

[TestFixture]
public class ChatRepositoryTests
{
    [Test]
    public async Task GetAllAsync_ReturnsEntities()
    {
        var context = new ChatDbContext(await UnitTestHelper.GetInMemoryDbOptionsAsync());
        var repository = new ChatRepository(context);
        var expected = context.Chats.ToList();

        var actual = await repository.GetAllAsync();

        actual.Should().Equal(expected);
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public async Task GetByIdAsync_ReturnsSingleEntity(int id)
    {
        var context = new ChatDbContext(await UnitTestHelper.GetInMemoryDbOptionsAsync());
        var repository = new ChatRepository(context);
        var expected = context.Chats.FirstOrDefault(c => c.Id == id);

        var actual = await repository.GetByIdAsync(id);

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task CreateAsync_AddsEntityToDatabase()
    {
        var context = new ChatDbContext(await UnitTestHelper.GetInMemoryDbOptionsAsync());
        var repository = new ChatRepository(context);
        var chatEntity = new ChatBuilder().Build();
        var beforeCount = context.Chats.Count();

        await repository.CreateAsync(chatEntity);
        await context.SaveChangesAsync();

        context.Chats.Count().Should().Be(beforeCount + 1);
    }

    [Test]
    public async Task Update_UpdatesEntityInDatabase()
    {
        var context = new ChatDbContext(await UnitTestHelper.GetInMemoryDbOptionsAsync());
        var repository = new ChatRepository(context);
        var chatEntity = context.Chats.First();
        chatEntity.Name = "updated name";

        repository.Update(chatEntity);
        await context.SaveChangesAsync();

        context.Chats.First().Name.Should().Be("updated name");
    }

    [Test]
    public async Task Delete_RemovesEntityFromDatabase()
    {
        var context = new ChatDbContext(await UnitTestHelper.GetInMemoryDbOptionsAsync());
        var repository = new ChatRepository(context);
        var chatEntity = context.Chats.First();
        var beforeCount = context.Chats.Count();
        
        repository.Delete(chatEntity);
        await context.SaveChangesAsync();

        context.Chats.Count().Should().Be(beforeCount - 1);
    }

    [Test]
    public async Task GetTotalCountAsync_ReturnsEntitiesCount()
    {
        var context = new ChatDbContext(await UnitTestHelper.GetInMemoryDbOptionsAsync());
        var repository = new ChatRepository(context);
        var expected = context.Chats.Count();

        var actual = await repository.GetTotalCountAsync(new ChatFilterModel());

        actual.Should().Be(expected);
    }
}