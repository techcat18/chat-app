using Bogus;
using ChatApplication.DAL.Entities;

namespace ChatApplication.IntegrationTests.DummyData;

public class UserEntityGenerator
{
    private readonly Faker<User> _faker;

    public UserEntityGenerator()
    {
        _faker = new Faker<User>()
            .RuleFor(u => u.FirstName, faker => faker.Person.FirstName)
            .RuleFor(u => u.LastName, faker => faker.Person.LastName)
            .RuleFor(u => u.Age, faker => faker.Random.Int())
            .RuleFor(u => u.Email, faker => faker.Person.Email)
            .RuleFor(u => u.NormalizedEmail, faker => faker.Person.Email.ToUpper())
            .RuleFor(u => u.SecurityStamp, faker => faker.Random.String2(10))
            .RuleFor(u => u.ConcurrencyStamp, faker => faker.Random.Guid().ToString())
            .RuleFor(u => u.UserName, faker => faker.Person.UserName)
            .RuleFor(u => u.UserName, faker => faker.Person.UserName.ToUpper())
            .RuleFor(u => u.PasswordHash,
                faker => "AQAAAAEAACcQAAAAEEAsBD6RrGDOg+0YbFA9ErzXca7pIqAlTZHO1DsW7aM53xXPr9ctqbBQmincyaMY0w==");
    }

    public User Generate()
    {
        return _faker
            .Generate();
    }

    public IEnumerable<User> Generate(int n)
    {
        return _faker
            .Generate(n);
    }
}