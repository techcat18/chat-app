using ChatApplication.BLL.Tests.DummyDataGenerators.User;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Models.User;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ChatApplication.BLL.Tests.Services.UserService.GetByIdAsync;

[TestFixture]
public class GivenValidInput: UserServiceBaseSetup
{
    private User _userEntity;
    private UserModel _expected;
    private UserModel _actual;
    private readonly string _userId = "id";
    
    [OneTimeSetUp]
    public async Task Init()
    {
        _userEntity = new UserEntityGenerator().Generate();
        _expected = new UserModelGenerator().Generate();

        _mockUnitOfWork
            .Setup(uow => uow.GetRepository<IUserRepository>().GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(_userEntity);

        _mockMapper
            .Setup(mapper => mapper.Map<UserModel>(_userEntity))
            .Returns(_expected);

        _actual = await _sut.GetByIdAsync(_userId);
    }

    [Test]
    public void ThenUserRepositoryGetByIdAsyncIsCalledOnce()
    {
        _mockUnitOfWork
            .Verify(uow => uow.GetRepository<IUserRepository>().GetByIdAsync(_userId, CancellationToken.None), Times.Once());
    }
    
    [Test]
    public void ThenActualMatchesExpected()
    {
        _actual.Should().BeEquivalentTo(_expected);
    }
}