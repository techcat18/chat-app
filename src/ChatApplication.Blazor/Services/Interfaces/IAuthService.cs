using ChatApplication.Blazor.Models.Auth;

namespace ChatApplication.Blazor.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseModel> LoginAsync(LoginModel loginModel);
    Task<AuthResponseModel> SignupAsync(SignupModel signupModel);
    Task<AuthResponseModel> ChangePasswordAsync(ChangePasswordModel changePasswordModel);
    Task<AuthResponseModel> ChangeInfoAsync(ChangeUserInfoModel changeUserInfoModel);
    Task LogoutAsync();
}