namespace ChatApplication.BLL.Exceptions.Auth;

public class AuthException: Exception
{
    public AuthException(string message): base(message){}
}