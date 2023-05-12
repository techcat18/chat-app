using Bogus;
using ChatApplication.Shared.Models.User;

namespace ChatApplication.BLL.Tests.DummyDataGenerators.User;

public class UserModelGenerator
{
    private readonly Faker<UserModel> _faker;
    private readonly List<int> _chatIds = new (){ 1, 2, 3, 4, 5, 6 };

    public UserModelGenerator()
    {
        _faker = new Faker<UserModel>()
            .RuleFor(u => u.Id, faker => faker.Random.Guid().ToString())
            .RuleFor(u => u.FirstName, faker => faker.Person.FirstName)
            .RuleFor(u => u.LastName, faker => faker.Person.LastName)
            .RuleFor(u => u.Age, faker => faker.Random.Int())
            .RuleFor(u => u.Email, faker => faker.Person.Email);
    }

    public UserModel Generate()
    {
        return _faker
            .Generate();
    }

    public IEnumerable<UserModel> Generate(int n)
    {
        return _faker
            .Generate(n);
    }
}