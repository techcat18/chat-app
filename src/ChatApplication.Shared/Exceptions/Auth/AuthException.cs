namespace ChatApplication.Shared.Exceptions.Auth;

public class AuthException: Exception
{
    public AuthException(string message): base(message){}
}