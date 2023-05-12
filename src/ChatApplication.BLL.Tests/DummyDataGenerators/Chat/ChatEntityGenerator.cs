using Bogus;

namespace ChatApplication.BLL.Tests.DummyDataGenerators.Chat;

public class ChatEntityGenerator
{
    private readonly Faker<DAL.Entities.Chat> _faker;
    private readonly IEnumerable<int> _chatTypeIds = new List<int> { 0, 1 };

    public ChatEntityGenerator()
    {
        _faker = new Faker<DAL.Entities.Chat>()
            .CustomInstantiator(_ => new DAL.Entities.Chat())
            .RuleFor(cm => cm.Id, faker => faker.IndexFaker)
            .RuleFor(cm => cm.Name, faker => faker.Name.JobArea())
            .RuleFor(cm => cm.ChatTypeId, faker => faker.PickRandom(_chatTypeIds));
    }

    public DAL.Entities.Chat Generate()
    {
        return _faker
            .Generate();
    }

    public IEnumerable<DAL.Entities.Chat> Generate(int n)
    {
        return _faker
            .Generate(n);
    }
}