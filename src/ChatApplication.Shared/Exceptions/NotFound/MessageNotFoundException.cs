namespace ChatApplication.Shared.Exceptions.NotFound;

public class MessageNotFoundException: NotFoundException
{
    public MessageNotFoundException(int id) : base($"Message with id {id} was not found")
    {
    }
}