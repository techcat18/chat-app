namespace ChatApplication.BLL.Exceptions.NotFound;

public class ChatNotFoundException: NotFoundException
{
    public ChatNotFoundException(string message): base(message)
    {
    }
}