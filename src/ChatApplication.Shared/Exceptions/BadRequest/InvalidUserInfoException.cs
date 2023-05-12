namespace ChatApplication.Shared.Exceptions.BadRequest;

public class InvalidUserInfoException: BadRequestException
{
    public InvalidUserInfoException(string message) : base(message)
    {
    }
}