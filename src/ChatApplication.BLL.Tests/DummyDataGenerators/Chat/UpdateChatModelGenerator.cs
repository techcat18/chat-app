using Bogus;
using ChatApplication.Shared.Models.Chat;

namespace ChatApplication.BLL.Tests.DummyDataGenerators.Chat;

public class UpdateChatModelGenerator
{
    private readonly Faker<UpdateChatModel> _faker;
    private readonly IEnumerable<int> _chatTypeIds = new List<int> { 0, 1 };

    public UpdateChatModelGenerator()
    {
        _faker = new Faker<UpdateChatModel>()
            .CustomInstantiator(_ => new UpdateChatModel())
            .RuleFor(cm => cm.Name, faker => faker.Name.JobArea())
            .RuleFor(cm => cm.ChatTypeId, faker => faker.PickRandom(_chatTypeIds));
    }

    public UpdateChatModel Generate()
    {
        return _faker
            .Generate();
    }

    public IEnumerable<UpdateChatModel> Generate(int n)
    {
        return _faker
            .Generate(n);
    }
}