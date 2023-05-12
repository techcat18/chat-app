using ChatApplication.BLL.Tests.DummyDataGenerators.User;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Exceptions.BadRequest;
using ChatApplication.Shared.Models.User;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ChatApplication.BLL.Tests.Services.UserService.UpdateAsync;

[TestFixture]
public class GivenEmailThatExists: UserServiceBaseSetup
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
            .Setup(uow => uow.GetRepository<IUserRepository>().GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(_userEntity);

        _mockUnitOfWork
            .Setup(uow =>
                uow.GetRepository<IUserRepository>().GetByEmailAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new User());

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
    public void ThenUserAlreadyExistsExceptionIsThrown()
    {
        _exception.Should().BeOfType<UserAlreadyExistsException>();
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