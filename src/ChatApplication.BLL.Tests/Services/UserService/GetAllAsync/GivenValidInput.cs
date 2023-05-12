using ChatApplication.BLL.Tests.DummyDataGenerators.User;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Models.User;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ChatApplication.BLL.Tests.Services.UserService.GetAllAsync;

[TestFixture]
public class GivenValidInput: UserServiceBaseSetup
{
    private IEnumerable<User> _userEntities;
    private IEnumerable<UserModel> _expected;
    private IEnumerable<UserModel> _actual;

    [OneTimeSetUp]
    public async Task Init()
    {
        _userEntities = new UserEntityGenerator().Generate(10);
        _expected = new UserModelGenerator().Generate(10);

        _mockUnitOfWork
            .Setup(uow => uow.GetRepository<IUserRepository>().GetAllAsync(CancellationToken.None))
            .ReturnsAsync(_userEntities);

        _mockMapper
            .Setup(mapper => mapper.Map<IEnumerable<UserModel>>(It.IsAny<IEnumerable<User>>()))
            .Returns(_expected);

        _actual = await _sut.GetAllAsync();
    }

    [Test]
    public void ThenUserRepositoryGetAllAsyncIsCalledOnce()
    {
        _mockUnitOfWork
            .Verify(uow => uow.GetRepository<IUserRepository>().GetAllAsync(CancellationToken.None), Times.Once);
    }
    
    [Test]
    public void ThenActualMatchExpected()
    {
        _actual.Should().Equal(_expected);
    }
}