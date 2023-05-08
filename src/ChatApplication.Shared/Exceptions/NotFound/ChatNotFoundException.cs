namespace ChatApplication.Shared.Exceptions.NotFound;

public class ChatNotFoundException: NotFoundException
{
    public ChatNotFoundException(string message): base(message)
    {
    }
}