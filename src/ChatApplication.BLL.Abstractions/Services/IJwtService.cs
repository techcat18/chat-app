using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace ChatApplication.BLL.Abstractions.Services;

public interface IJwtService
{
    SigningCredentials GetSigningCredentials();
    Task<List<Claim>> GetClaimsAsync(string userId);
    JwtSecurityToken GenerateToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims);
}