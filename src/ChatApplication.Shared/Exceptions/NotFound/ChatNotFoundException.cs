namespace ChatApplication.Shared.Exceptions.NotFound;

public class ChatNotFoundException: NotFoundException
{
    public ChatNotFoundException(string message): base(message)
    {
    }

    public ChatNotFoundException(int id) : base($"Chat with id {id} was not found")
    {
    }
}