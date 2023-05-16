using ChatApplication.BLL.Services;
using ChatApplication.DAL.Data;
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
        
    }
}