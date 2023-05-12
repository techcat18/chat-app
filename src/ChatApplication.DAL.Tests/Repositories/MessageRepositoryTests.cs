using ChatApplication.DAL.Data;
using ChatApplication.DAL.Repositories;
using ChatApplication.DAL.Tests.Builders.Message;
using FluentAssertions;
using NUnit.Framework;

namespace ChatApplication.DAL.Tests.Repositories;

[TestFixture]
public class MessageRepositoryTests
{
    [Test]
    public async Task GetAllAsync_ReturnsEntities()
    {
        var context = new ChatDbContext(await UnitTestHelper.GetInMemoryDbOptionsAsync());
        var repository = new MessageRepository(context);
        var expected = context.Messages.ToList();

        var actual = await repository.GetAllAsync();

        actual.Should().Equal(expected);
    }

    /*[TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public async Task GetByIdAsync_ReturnsSingleEntity(int id)
    {
        var context = new ChatDbContext(await UnitTestHelper.GetInMemoryDbOptionsAsync());
        var repository = new MessageRepository(context);
        var expected = context.Messages.FirstOrDefault(m => m.Id == id);

        var actual = await repository.GetByIdAsync(id);

        actual.Should().BeEquivalentTo(expected);
    }*/

    [Test]
    public async Task CreateAsync_AddsEntityToDatabase()
    {
        var context = new ChatDbContext(await UnitTestHelper.GetInMemoryDbOptionsAsync());
        var repository = new MessageRepository(context);
        var messageEntity = new MessageBuilder().Build();

        await repository.CreateAsync(messageEntity);
        await context.SaveChangesAsync();

        context.Messages.Count().Should().Be(11);
    }
}