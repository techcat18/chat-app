using ChatApplication.BLL.Tests.DummyDataGenerators.User;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Models.User;
using Moq;
using NUnit.Framework;

namespace ChatApplication.BLL.Tests.Services.UserService.UpdateAsync;

[TestFixture]
public class GivenValidInput: UserServiceBaseSetup
{
    private User _userEntity;
    private UpdateUserModel _updateUserModel;
    
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
                uow.GetRepository<IUserRepository>().GetByEmailAsync(It.IsAny<string>(), CancellationToken.None));

        _mockUnitOfWork
            .Setup(uow => uow.GetRepository<IUserRepository>().Update(It.IsAny<User>()));

        await _sut.UpdateAsync(_updateUserModel);
    }
    
    [Test]
    public void ThenUserRepositoryUpdateAsyncIsCalledOnce()
    {
        _mockUnitOfWork
            .Verify(uow => uow.GetRepository<IUserRepository>().Update(_userEntity), Times.Once);
    }

    [Test]
    public void ThenContextSaveChangesAsyncIsCalledOnce()
    {
        _mockUnitOfWork
            .Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
}