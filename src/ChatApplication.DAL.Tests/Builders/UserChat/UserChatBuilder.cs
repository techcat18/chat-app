using Bogus;

namespace ChatApplication.DAL.Tests.Builders.UserChat;

public class UserChatBuilder
{
    private readonly Faker<Entities.UserChat> _faker;

    public UserChatBuilder()
    {
        _faker = new Faker<Entities.UserChat>()
            .CustomInstantiator(_ => new Entities.UserChat())
            .RuleFor(uc => uc.ChatId, faker => faker.PickRandom(new List<int> { 1, 2, 3, 4, 5, 6 }))
            .RuleFor(uc => uc.UserId, faker => faker.Random.Guid().ToString());
    }

    public Entities.UserChat Build()
    {
        return _faker
            .Generate();
    }

    public IEnumerable<Entities.UserChat> Build(int n)
    {
        return _faker
            .Generate(n);
    }
}