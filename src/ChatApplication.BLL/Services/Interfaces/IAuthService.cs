using ChatApplication.BLL.Models.Auth;

namespace ChatApplication.BLL.Services.Interfaces;

public interface IAuthService
{
    Task<JwtModel> LoginAsync(LoginModel model);
    Task SignupAsync(SignupModel model);
}