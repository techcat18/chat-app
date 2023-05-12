using Bogus;

namespace ChatApplication.BLL.Tests.DummyDataGenerators.Message;

public class MessageEntityGenerator
{
    private readonly Faker<DAL.Entities.Message> _faker;

    public MessageEntityGenerator()
    {
        _faker = new Faker<DAL.Entities.Message>()
            .CustomInstantiator(_ => new DAL.Entities.Message())
            .RuleFor(m => m.ChatId, faker => faker.Random.Int())
            .RuleFor(m => m.SenderId, faker => faker.Random.String())
            .RuleFor(m => m.Content, faker => faker.Commerce.ProductDescription());
    }

    public DAL.Entities.Message Generate()
    {
        return _faker
            .Generate();
    }

    public IEnumerable<DAL.Entities.Message> Generate(int n)
    {
        return _faker
            .Generate(n);
    }
}