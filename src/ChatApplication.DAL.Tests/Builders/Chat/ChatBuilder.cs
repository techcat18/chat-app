using Bogus;

namespace ChatApplication.DAL.Tests.Builders.Chat;

public class ChatBuilder
{
    private readonly Faker<DAL.Entities.Chat> _faker;
    private readonly IEnumerable<int> _chatTypeIds = new List<int> { 0, 1 };

    public ChatBuilder()
    {
        _faker = new Faker<DAL.Entities.Chat>()
            .CustomInstantiator(_ => new DAL.Entities.Chat())
            .RuleFor(cm => cm.Name, faker => faker.Name.JobArea())
            .RuleFor(cm => cm.ChatTypeId, faker => faker.PickRandom(_chatTypeIds));
    }

    public DAL.Entities.Chat Build()
    {
        return _faker
            .Generate();
    }

    public IEnumerable<DAL.Entities.Chat> Build(int n)
    {
        return _faker
            .Generate(n);
    }
}