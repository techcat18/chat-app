using Bogus;
using ChatApplication.Shared.Models.User;

namespace ChatApplication.BLL.Tests.DummyDataGenerators.User;

public class UpdateUserModelGenerator
{
    private readonly Faker<UpdateUserModel> _faker;
    private readonly List<int> _chatIds = new (){ 1, 2, 3, 4, 5, 6 };

    public UpdateUserModelGenerator()
    {
        _faker = new Faker<UpdateUserModel>()
            .RuleFor(u => u.FirstName, faker => faker.Person.FirstName)
            .RuleFor(u => u.LastName, faker => faker.Person.LastName)
            .RuleFor(u => u.Age, faker => faker.Random.Int())
            .RuleFor(u => u.Email, faker => faker.Person.Email);
    }

    public UpdateUserModel Generate()
    {
        return _faker
            .Generate();
    }

    public IEnumerable<UpdateUserModel> Generate(int n)
    {
        return _faker
            .Generate(n);
    }
}