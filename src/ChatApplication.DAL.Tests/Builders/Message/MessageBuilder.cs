using Bogus;

namespace ChatApplication.DAL.Tests.Builders.Message;

public class MessageBuilder
{
    private readonly Faker<Entities.Message> _faker;
    private readonly IEnumerable<int> _chatIds = new List<int>{1, 2, 3};

    public MessageBuilder()
    {
        _faker = new Faker<Entities.Message>()
            .CustomInstantiator(_ => new Entities.Message())
            .RuleFor(m => m.ChatId, faker => faker.PickRandom(_chatIds))
            .RuleFor(m => m.SenderId, faker => faker.Random.String())
            .RuleFor(m => m.Content, faker => faker.Commerce.ProductDescription());
    }

    public Entities.Message Build()
    {
        return _faker
            .Generate();
    }

    public IEnumerable<Entities.Message> Build(int n)
    {
        return _faker
            .Generate(n);
    }
}