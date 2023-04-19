using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using ChatApplication.Blazor.Helpers.Interfaces;
using ChatApplication.Blazor.Models.Auth;
using ChatApplication.Blazor.Services.Interfaces;
using Flurl.Http;

namespace ChatApplication.Blazor.Services;

public class AuthService: IAuthService
{
    private readonly ChatAuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;
    private readonly IApiHelper _apiHelper;
    
    public AuthService(
        ChatAuthenticationStateProvider authenticationStateProvider,
        ILocalStorageService localStorage,
        IApiHelper apiHelper)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorage;
        _apiHelper = apiHelper;
    }

    public async Task<LoginResponseModel> LoginAsync(LoginModel loginModel)
    {
        try
        {
            var jwtModel = 
                await _apiHelper.PostAsync<LoginModel, JwtModel>(loginModel, "auth/login");

            await _localStorage.SetItemAsync("token", jwtModel.Token);
            await _authenticationStateProvider.MarkUserAsAuthenticated(jwtModel.Token);
            
            return new LoginResponseModel { Succeeded = true, JwtToken = jwtModel.Token };
        }
        catch (FlurlHttpException e)
        {
            return new LoginResponseModel { Succeeded = false, ErrorMessage = e.Message };
        }
    }
    
    private static IEnumerable<Claim> ParseClaimsFromJwtToken(string token)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtToken = jwtHandler.ReadJwtToken(token);
        
        var claims = jwtToken.Claims.Select(c => new Claim(c.Type, c.Value));

        return claims;
    }

    public async Task<SignupResponseModel> SignupAsync(SignupModel signupModel)
    {
        try
        {
            await _apiHelper.PostAsync(signupModel, "auth/signup");

            return new SignupResponseModel { Succeeded = true };
        }
        catch (FlurlHttpException e)
        {
            return new SignupResponseModel { Succeeded = false, ErrorMessage = e.Message };
        }
    }

    public async Task<ChangePasswordResponseModel> ChangePasswordAsync(ChangePasswordModel changePasswordModel)
    {
        try
        {
            await _apiHelper.PutAsync<ChangePasswordModel, ChangePasswordResponseModel>(changePasswordModel, "auth/changePassword");

            return new ChangePasswordResponseModel { Succeeded = true };
        }
        catch (FlurlHttpException e)
        {
            return new ChangePasswordResponseModel { Succeeded = false, ErrorMessage = e.Message };
        }
        
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("token");
        await _authenticationStateProvider.MarkUserAsLoggedOut();
    }
}