using Bogus;
using ChatApplication.Shared.Models.Chat;

namespace ChatApplication.BLL.Tests.DummyDataGenerators.Chat;

public class CreateChatModelGenerator
{
    private readonly Faker<CreateChatModel> _faker;
    private readonly IEnumerable<int> _chatTypeIds = new List<int> { 0, 1 };

    public CreateChatModelGenerator()
    {
        _faker = new Faker<CreateChatModel>()
            .CustomInstantiator(_ => new CreateChatModel())
            .RuleFor(cm => cm.Name, faker => faker.Name.JobArea())
            .RuleFor(cm => cm.ChatTypeId, faker => faker.PickRandom(_chatTypeIds));
    }

    public CreateChatModel Generate()
    {
        return _faker
            .Generate();
    }

    public IEnumerable<CreateChatModel> Generate(int n)
    {
        return _faker
            .Generate(n);
    }
}