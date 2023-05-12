using ChatApplication.BLL.Services;
using ChatApplication.DAL.Data;
using ChatApplication.IntegrationTests.Utility;
using ChatApplication.Shared.Models.Auth;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace ChatApplication.IntegrationTests.Services;

[TestFixture]
public class AuthServiceTests
{
    private AuthService _authService;
    private ChatDbContext _context;

    [OneTimeSetUp]
    public async Task Init()
    {
        var dbOptions = DbHelper.GetDbContextOptions();
        _context = await DbHelper.CreateChatDbContextAsync(dbOptions);

        var userManager = ServiceHelper.CreateUserManager(_context);
        var signInManager = ServiceHelper.CreateSignInManager(userManager);
        var mapper = ServiceHelper.CreateMapper();
        var jwtService = ServiceHelper.CreateJwtService(userManager);

        _authService = new AuthService(
            userManager,
            signInManager,
            mapper,
            jwtService);
    }

    [SetUp]
    public async Task TestSetup()
    {
        await _context.ClearData();
        await _context.SeedUsers();
    }

    [Test]
    public async Task LoginAsync_GivenValidCredentials_ReturnsJwtToken()
    {
        var user = await _context.Users.FirstAsync();
        var loginModel = new LoginModel { Email = user.Email, Password = "Kostia18@" };

        var actual = await _authService.LoginAsync(loginModel);

        actual.Should().NotBeNull();
    }
}