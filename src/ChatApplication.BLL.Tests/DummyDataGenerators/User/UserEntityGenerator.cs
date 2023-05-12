using Bogus;

namespace ChatApplication.BLL.Tests.DummyDataGenerators.User;

public class UserEntityGenerator
{
    private readonly Faker<DAL.Entities.User> _faker;

    public UserEntityGenerator()
    {
        _faker = new Faker<DAL.Entities.User>()
            .RuleFor(u => u.Id, faker => faker.Random.Guid().ToString())
            .RuleFor(u => u.FirstName, faker => faker.Person.FirstName)
            .RuleFor(u => u.LastName, faker => faker.Person.LastName)
            .RuleFor(u => u.Age, faker => faker.Random.Int())
            .RuleFor(u => u.Email, faker => faker.Person.Email);
    }

    public DAL.Entities.User Generate()
    {
        return _faker
            .Generate();
    }

    public IEnumerable<DAL.Entities.User> Generate(int n)
    {
        return _faker
            .Generate(n);
    }
}