using ChatApplication.Blazor.Models.Auth;

namespace ChatApplication.Blazor.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseModel> LoginAsync(LoginModel loginModel);
    Task<SignupResponseModel> SignupAsync(SignupModel signupModel);
    Task LogoutAsync();
}