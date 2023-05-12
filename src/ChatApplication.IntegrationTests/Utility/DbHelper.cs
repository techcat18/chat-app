using ChatApplication.DAL.Data;
using ChatApplication.DAL.Entities;
using ChatApplication.IntegrationTests.DummyData;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.IntegrationTests.Utility;

public static class DbHelper
{
    public static DbContextOptions<ChatDbContext> GetDbContextOptions()
    {
        var options = new DbContextOptionsBuilder<ChatDbContext>()
            .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ChatTestDb;Integrated Security=true;")
            .EnableSensitiveDataLogging()
            .Options;
        
        return options;
    }

    public static async Task<ChatDbContext> CreateChatDbContextAsync(DbContextOptions<ChatDbContext> options)
    {
        var context = new ChatDbContext(options);
        
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        await context.InitDbAsync();
        
        return context;
    }

    private static async Task InitDbAsync(this ChatDbContext context)
    {
        var dirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\SQLScripts");

        foreach (var file in Directory.EnumerateFiles(dirPath))
        {
            var script = await File.ReadAllTextAsync(file);
            await context.Database.ExecuteSqlRawAsync(script);
        }
    }

    public static async Task SeedChatTypesAsync(this ChatDbContext context)
    {
        var chatTypes = new List<ChatType>{ new() { Name = "Public" }, new() { Name = "Private" } };
        
        await context.ChatTypes.AddRangeAsync(chatTypes);
        await context.SaveChangesAsync();
    }

    public static async Task SeedChats(this ChatDbContext context)
    {
        var chatTypes = await context.ChatTypes.ToListAsync();
        var chats = new ChatEntityGenerator(chatTypes.Select(ct => ct.Id)).Generate(20);
        
        await context.Chats.AddRangeAsync(chats);
        await context.SaveChangesAsync();
    }

    public static async Task SeedUsers(this ChatDbContext context)
    {
        var users = new UserEntityGenerator().Generate(10);

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();
    }

    public static async Task SeedUserChats(this ChatDbContext context)
    {
        var users = context.Users.ToList();
        
        if (!users.Any())
        {
            throw new Exception("Users are required for message seeding");
        }
        
        var chats = context.Chats.ToList();
        
        if (!chats.Any())
        {
            throw new Exception("Chats are required for message seeding");
        }

        var userChats = new List<UserChat>();

        for (var i = 0; i < chats.Count / 2; i++)
        {
            for (var j = users.Count / 2; j < users.Count; j++)
            {
                userChats.Add(new UserChat {ChatId = chats[i].Id, UserId = users[j].Id});
            }
        }

        await context.AddRangeAsync(userChats);
    }

    public static async Task SeedMessages(this ChatDbContext context)
    {
        var chats = await context.Chats.ToListAsync();

        if (!chats.Any())
        {
            throw new Exception("Chats are required for message seeding");
        }

        var users = await context.Users.ToListAsync();

        if (!users.Any())
        {
            throw new Exception("Users are required for message seeding");
        }

        var messages = new MessageEntityGenerator(
                chats.Select(c => c.Id),
                users.Select(u => u.Id))
            .Generate(100);

        await context.Messages.AddRangeAsync(messages);
        await context.SaveChangesAsync();
    }

    public static async Task ClearData(this ChatDbContext context)
    {
        context.ChatTypes.RemoveRange(context.ChatTypes);
        context.Chats.RemoveRange(context.Chats);
        context.Users.RemoveRange(context.Users);
        context.Messages.RemoveRange(context.Messages);

        await context.SaveChangesAsync();
    }
}