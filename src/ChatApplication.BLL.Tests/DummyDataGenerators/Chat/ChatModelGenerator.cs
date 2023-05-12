using Bogus;
using ChatApplication.Shared.Models.Chat;

namespace ChatApplication.BLL.Tests.DummyDataGenerators.Chat;

public class ChatModelGenerator
{
    private readonly Faker<ChatModel> _faker;
    private readonly IEnumerable<int> _chatTypeIds = new List<int> { 0, 1 };

    public ChatModelGenerator()
    {
        _faker = new Faker<ChatModel>()
            .CustomInstantiator(_ => new ChatModel())
            .RuleFor(cm => cm.Id, faker => faker.IndexFaker)
            .RuleFor(cm => cm.Name, faker => faker.Name.JobArea())
            .RuleFor(cm => cm.MembersCount, faker => faker.Random.Int())
            .RuleFor(cm => cm.ChatTypeId, faker => faker.PickRandom(_chatTypeIds));
    }

    public ChatModel Generate()
    {
        return _faker
            .Generate();
    }

    public IEnumerable<ChatModel> Generate(int n)
    {
        return _faker
            .Generate(n);
    }
}