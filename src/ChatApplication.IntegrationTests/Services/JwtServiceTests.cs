using System.Security.Claims;
using ChatApplication.BLL.Services;
using ChatApplication.DAL.Data;
using ChatApplication.DAL.Entities;
using ChatApplication.IntegrationTests.Utility;
using ChatApplication.Shared.Models.Auth;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;

namespace ChatApplication.IntegrationTests.Services;

[TestFixture]
public class JwtServiceTests
{
    private JwtService _jwtService;
    private IOptions<JwtSettings> _jwtSettings;
    private ChatDbContext _context;

    [SetUp]
    public async Task Init()
    {
        var dbOptions = DbHelper.GetDbContextOptions();
        _context = await DbHelper.CreateChatDbContextAsync(dbOptions);

        var serviceProvider = ServiceHelper.RegisterServices(_context);
        
        _jwtSettings = Options.Create(new JwtSettings 
        {
            Key = "testKey",
            Issuer = "testIssuer",
            Audience = "testAudience"
        });

        var userManager = ServiceHelper.CreateUserManager(serviceProvider);

        _jwtService = new JwtService(_jwtSettings, userManager);
    }
    
    [SetUp]
    public async Task TestSetup()
    {
        await _context.ClearData();
        await _context.SeedUsers();
    }

    [Test]
    public void GetSigningCredentials_ReturnsSigningCredentials()
    {
        var actual = _jwtService.GetSigningCredentials();

        actual.Key.Should().NotBeNull();
        actual.Algorithm.Should().Be(SecurityAlgorithms.HmacSha256);
    }

    [Test]
    public async Task GetClaimsAsync_ReturnsClaims()
    {
        var user = _context.Users.First();
        var actual = await _jwtService.GetClaimsAsync(user.Id);

        actual.Should().ContainSingle(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == user.Id);
        actual.Should().ContainSingle(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == user.Email);
        actual.Should().ContainSingle(c => c.Type == ClaimTypes.Role && c.Value == "User");
    }

    [Test]
    public void GenerateToken_ReturnsJwtSecurityToken()
    {
        var signingCredentials = _jwtService.GetSigningCredentials();
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, "1"),
            new(JwtRegisteredClaimNames.Email, "test@example.com"),
            new(ClaimTypes.Role, "User")
        };

        var actual = _jwtService.GenerateToken(signingCredentials, claims);

        actual.Should().NotBeNull();
        actual.Issuer.Should().Be(_jwtSettings.Value.Issuer);
        actual.Claims.Should().ContainSingle(c => 
                c.Type == JwtRegisteredClaimNames.Sub && c.Value == "1")
            .And.ContainSingle(c => 
                c.Type == JwtRegisteredClaimNames.Email && c.Value == "test@example.com")
            .And.ContainSingle(c => 
                c.Type == ClaimTypes.Role && c.Value == "User");
        actual.SignatureAlgorithm.Should().Be(SecurityAlgorithms.HmacSha256);
    }
}
