namespace ChatApplication.BLL.Exceptions.NotFound;

public class GroupChatNotFoundException: NotFoundException
{
    public GroupChatNotFoundException(string message): base(message)
    {
    }
}