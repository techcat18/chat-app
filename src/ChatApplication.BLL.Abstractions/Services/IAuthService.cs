using ChatApplication.Shared.Models.Auth;

namespace ChatApplication.BLL.Abstractions.Services;

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