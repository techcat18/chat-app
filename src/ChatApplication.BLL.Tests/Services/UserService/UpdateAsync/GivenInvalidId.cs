using ChatApplication.BLL.Tests.DummyDataGenerators.User;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Exceptions.NotFound;
using ChatApplication.Shared.Models.User;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ChatApplication.BLL.Tests.Services.UserService.UpdateAsync;

[TestFixture]
public class GivenInvalidId: UserServiceBaseSetup
{
    private User _userEntity;
    private UpdateUserModel _updateUserModel;
    private Exception _exception;
    
    [OneTimeSetUp]
    public async Task Init()
    {
        _userEntity = new UserEntityGenerator().Generate();
        _updateUserModel = new UpdateUserModelGenerator().Generate();

        _mockUnitOfWork
            .Setup(uow =>
                uow.GetRepository<IUserRepository>().GetByIdAsync(It.IsAny<string>(), CancellationToken.None));

        _mockUnitOfWork
            .Setup(uow => uow.GetRepository<IUserRepository>().Update(It.IsAny<User>()));

        try
        {
            await _sut.UpdateAsync(_updateUserModel);
        }
        catch (Exception e)
        {
            _exception = e;
        }
    }

    [Test]
    public void ThenUserNotFoundExceptionIsThrown()
    {
        _exception.Should().BeOfType<UserNotFoundException>();
    }
    
    [Test]
    public void ThenUserRepositoryUpdateAsyncIsNeverThrown()
    {
        _mockUnitOfWork
            .Verify(uow => uow.GetRepository<IUserRepository>().Update(It.IsAny<User>()), Times.Never());
    }

    [Test]
    public void ThenContextSaveChangesAsyncIsNeverCalled()
    {
        _mockUnitOfWork
            .Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Never);
    }
}