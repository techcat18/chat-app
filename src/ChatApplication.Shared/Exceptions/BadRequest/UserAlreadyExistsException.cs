namespace ChatApplication.Shared.Exceptions.BadRequest;

public class UserAlreadyExistsException: BadRequestException
{
    public UserAlreadyExistsException(string message) : base(message)
    {
    }
}