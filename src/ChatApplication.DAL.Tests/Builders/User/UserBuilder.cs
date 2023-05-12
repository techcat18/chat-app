using Bogus;
using ChatApplication.DAL.Entities;

namespace ChatApplication.DAL.Tests.Builders.User;

public class UserBuilder
{
   private readonly Faker<Entities.User> _faker;
   private readonly List<int> _chatIds = new (){ 1, 2, 3, 4, 5, 6 };

   public UserBuilder()
   {
      _faker = new Faker<Entities.User>()
         .RuleFor(u => u.Id, faker => faker.Random.Guid().ToString())
         .RuleFor(u => u.FirstName, faker => faker.Person.FirstName)
         .RuleFor(u => u.LastName, faker => faker.Person.LastName)
         .RuleFor(u => u.Age, faker => faker.Random.Int())
         .RuleFor(u => u.Email, faker => faker.Person.Email);
   }

   public Entities.User Build()
   {
      return _faker
         .Generate();
   }

   public IEnumerable<Entities.User> Build(int n)
   {
      return _faker
         .Generate(n);
   }
}