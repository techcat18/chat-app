using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Exceptions.NotFound;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ChatApplication.BLL.Tests.Services.UserService.GetByIdAsync;

[TestFixture]
public class GivenInvalidId: UserServiceBaseSetup
{
    private readonly string _userId = "invalid id";
    private Exception _exception;

    [OneTimeSetUp]
    public async Task Init()
    {
        _mockUnitOfWork
            .Setup(uow =>
                uow.GetRepository<IUserRepository>().GetByIdAsync(It.IsAny<string>(), CancellationToken.None));
        try
        {
            await _sut.GetByIdAsync(_userId);
        }
        catch (Exception e)
        {
            _exception = e;
        }
    }

    [Test]
    public void ThenUserRepositoryGetByIdAsyncIsCalledOnce()
    {
        _mockUnitOfWork
            .Verify(uow => uow.GetRepository<IUserRepository>().GetByIdAsync(_userId, CancellationToken.None),
                Times.Once);
    }

    [Test]
    public void ThenUserNotFoundExceptionIsThrown()
    {
        _exception.Should().BeOfType<UserNotFoundException>();
    }
}