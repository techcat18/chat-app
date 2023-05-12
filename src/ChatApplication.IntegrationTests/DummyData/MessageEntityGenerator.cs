using Bogus;
using ChatApplication.DAL.Entities;

namespace ChatApplication.IntegrationTests.DummyData;

public class MessageEntityGenerator
{
    private readonly Faker<Message> _faker;

    public MessageEntityGenerator(IEnumerable<int> chatIds, IEnumerable<string> userIds)
    {
        _faker = new Faker<Message>()
            .RuleFor(m => m.ChatId, faker => faker.PickRandom(chatIds))
            .RuleFor(m => m.SenderId, faker => faker.PickRandom(userIds))
            .RuleFor(m => m.Content, faker => faker.Commerce.ProductDescription());
    }

    public Message Generate()
    {
        return _faker
            .Generate();
    }

    public IEnumerable<Message> Generate(int n)
    {
        return _faker
            .Generate(n);
    }
}