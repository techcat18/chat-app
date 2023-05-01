using ChatApplication.BLL.Models.Auth;

namespace ChatApplication.BLL.Services.Interfaces;

public interface IAuthService
{
    Task<JwtModel> LoginAsync(
        LoginModel model,
        CancellationToken cancellationToken = default);
    
    Task SignupAsync(
        SignupModel model,
        CancellationToken cancellationToken = default);
    
    Task ResetPasswordAsync(
        ChangePasswordModel model,
        CancellationToken cancellationToken = default);
}