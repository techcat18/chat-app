using Bogus;
using ChatApplication.DAL.Entities;

namespace ChatApplication.IntegrationTests.DummyData;

public class ChatEntityGenerator
{
    private readonly Faker<Chat> _faker;

    public ChatEntityGenerator(IEnumerable<int> chatTypeIds)
    {
        _faker = new Faker<Chat>()
            .RuleFor(c => c.Name, faker => faker.Name.JobArea())
            .RuleFor(c => c.ChatTypeId, faker => faker.PickRandom(chatTypeIds));
    }

    public Chat Generate()
    {
        return _faker
            .Generate();
    }

    public IEnumerable<Chat> Generate(int n)
    {
        return _faker
            .Generate(n);
    }
}