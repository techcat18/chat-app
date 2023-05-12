using ChatApplication.BLL.Services;
using ChatApplication.DAL.Data;
using ChatApplication.DAL.Entities;
using ChatApplication.IntegrationTests.Utility;
using ChatApplication.Shared.Models.Auth;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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

        var serviceProvider = ServiceHelper.RegisterServices(_context);

        var userManager = ServiceHelper.CreateUserManager(serviceProvider);

        var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
        var signInManager = new SignInManager<User>(userManager, httpContextAccessor, new Mock<IUserClaimsPrincipalFactory<User>>().Object, null, null, null, null);
        signInManager.Context = httpContextAccessor.HttpContext;
        
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