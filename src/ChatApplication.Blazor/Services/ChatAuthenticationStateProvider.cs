using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace ChatApplication.Blazor.Services;

public class ChatAuthenticationStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal _user;
    private readonly ILocalStorageService _localStorage;

    public ChatAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task MarkUserAsAuthenticated(string jwt)
    {
        var token = await _localStorage.GetItemAsync<string>("token");
        
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        
        _user = new ClaimsPrincipal(identity);
        
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _localStorage.RemoveItemAsync("token");
        
        _user = new ClaimsPrincipal(new ClaimsIdentity());
        
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("token");

        if (token == null)
        {
            return new AuthenticationState(new ClaimsPrincipal());
        }
        
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        
        var user = new ClaimsPrincipal(identity);
        
        return new AuthenticationState(user);
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var claimsPrincipal = tokenHandler.ReadJwtToken(jwt);
        
        return claimsPrincipal.Claims;
    }
}
