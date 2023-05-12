using ChatApplication.DAL.Data;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Entities.Views;
using ChatApplication.DAL.Tests.Builders.Chat;
using ChatApplication.DAL.Tests.Builders.Message;
using ChatApplication.DAL.Tests.Builders.User;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.DAL.Tests;

public static class UnitTestHelper
{
    public static async Task<DbContextOptions<ChatDbContext>> GetInMemoryDbOptionsAsync()
    {
        var options = new DbContextOptionsBuilder<ChatDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

        await using var context = new ChatDbContext(options);
        await SeedDataAsync(context);

        return options;
    }

    private static async Task SeedDataAsync(ChatDbContext context)
    {
        var messageEntities = new MessageBuilder().Build(10).ToList();
        await context.Messages.AddRangeAsync(messageEntities);

        var chatEntities = new ChatBuilder().Build(10).ToList();
        await context.Chats.AddRangeAsync(chatEntities);

        var userEntities = new UserBuilder().Build(10).ToList();
        var userViews = userEntities.Select(u => new UserView
            { Id = u.Id, FirstName = u.FirstName, LastName = u.LastName, Age = u.Age, Email = u.Email, Image = u.Image});
        await context.Users.AddRangeAsync(userEntities);
        await context.UserView.AddRangeAsync(userViews);

        var userChats = userEntities.Select(u => new UserChat { UserId = u.Id, ChatId = chatEntities.First().Id });
        await context.UserChats.AddRangeAsync(userChats);
        
        await context.SaveChangesAsync();
    }
}