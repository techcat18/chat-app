namespace ChatApplication.Blazor.Models.Auth;

public class ChangePasswordModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}