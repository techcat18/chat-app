using Bogus;
using ChatApplication.DAL.Entities.Functions;

namespace ChatApplication.BLL.Tests.DummyDataGenerators.Message;

public class MessageFunctionGenerator
{
    private readonly Faker<MessageFunction> _faker;

    public MessageFunctionGenerator()
    {
        _faker = new Faker<MessageFunction>()
            .CustomInstantiator(_ => new MessageFunction())
            .RuleFor(m => m.ChatId, faker => faker.Random.Int())
            .RuleFor(m => m.SenderId, faker => faker.Random.String())
            .RuleFor(m => m.Content, faker => faker.Commerce.ProductDescription());
    }

    public MessageFunction Generate()
    {
        return _faker
            .Generate();
    }

    public IEnumerable<MessageFunction> Generate(int n)
    {
        return _faker
            .Generate(n);
    }
}