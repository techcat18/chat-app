using Bogus;
using ChatApplication.Shared.Models.Message;

namespace ChatApplication.BLL.Tests.DummyDataGenerators.Message;

public class CreateMessageModelGenerator
{
    private readonly Faker<CreateMessageModel> _faker;
    
    public CreateMessageModelGenerator()
    {
        _faker = new Faker<CreateMessageModel>()
            .CustomInstantiator(_ => new CreateMessageModel())
            .RuleFor(m => m.ChatId, faker => faker.Random.Int())
            .RuleFor(m => m.SenderId, faker => faker.Random.String())
            .RuleFor(m => m.Content, faker => faker.Commerce.ProductDescription());
    }

    public CreateMessageModel Generate()
    {
        return _faker
            .Generate();
    }

    public IEnumerable<CreateMessageModel> Generate(int n)
    {
        return _faker
            .Generate(n);
    }
}