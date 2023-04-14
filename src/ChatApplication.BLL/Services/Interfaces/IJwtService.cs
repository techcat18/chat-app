using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ChatApplication.DAL.Entities;
using Microsoft.IdentityModel.Tokens;

namespace ChatApplication.BLL.Services.Interfaces;

public interface IJwtService
{
    SigningCredentials GetSigningCredentials();
    Task<List<Claim>> GetClaimsAsync(User user);
    JwtSecurityToken GenerateToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims);
}