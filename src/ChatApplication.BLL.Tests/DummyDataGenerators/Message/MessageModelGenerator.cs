using Bogus;
using ChatApplication.Shared.Models.Message;

namespace ChatApplication.BLL.Tests.DummyDataGenerators.Message;

public class MessageModelGenerator
{
    private readonly Faker<MessageModel> _faker;

    public MessageModelGenerator()
    {
        _faker = new Faker<MessageModel>()
            .CustomInstantiator(_ => new MessageModel())
            .RuleFor(m => m.ChatId, faker => faker.Random.Int())
            .RuleFor(m => m.SenderId, faker => faker.Random.String())
            .RuleFor(m => m.Content, faker => faker.Commerce.ProductDescription());
    }

    public MessageModel Generate()
    {
        return _faker
            .Generate();
    }

    public IEnumerable<MessageModel> Generate(int n)
    {
        return _faker
            .Generate(n);
    }
}