using ChatApplication.DAL.Data;
using ChatApplication.DAL.Repositories;
using ChatApplication.DAL.Tests.Builders.UserChat;
using FluentAssertions;
using NUnit.Framework;

namespace ChatApplication.DAL.Tests.Repositories;

[TestFixture]
public class UserChatRepositoryTests
{
    [Test]
    public async Task CreateAsync_AddsEntityToDatabase()
    {
        var context = new ChatDbContext(await UnitTestHelper.GetInMemoryDbOptionsAsync());
        var repository = new UserChatRepository(context);
        var entityToAdd = new UserChatBuilder().Build();
        var beforeCount = context.UserChats.Count();

        await repository.CreateAsync(entityToAdd);
        await context.SaveChangesAsync();

        context.UserChats.Count().Should().Be(beforeCount + 1);
    }

    [Test]
    public async Task CreateRangeAsync_AddsEntitiesToDatabase()
    {
        var context = new ChatDbContext(await UnitTestHelper.GetInMemoryDbOptionsAsync());
        var repository = new UserChatRepository(context);
        var entitiesToAdd = new UserChatBuilder().Build(10).ToList();
        var beforeCount = context.UserChats.Count();

        await repository.CreateRangeAsync(entitiesToAdd);
        await context.SaveChangesAsync();

        context.UserChats.Count().Should().Be(beforeCount + entitiesToAdd.Count);
    }
}