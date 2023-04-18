namespace ChatApplication.Blazor.Models.Auth;

public class LoginResponseModel
{
    public bool Succeeded { get; set; }
    public string JwtToken { get; set; }
    public string ErrorMessage { get; set; }
}