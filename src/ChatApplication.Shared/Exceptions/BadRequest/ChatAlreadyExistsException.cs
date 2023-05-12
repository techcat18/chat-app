namespace ChatApplication.Shared.Exceptions.BadRequest;

public class ChatAlreadyExistsException: BadRequestException
{
    public ChatAlreadyExistsException(string message) : base(message)
    {
    }
}