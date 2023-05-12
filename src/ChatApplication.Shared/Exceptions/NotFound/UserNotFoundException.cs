namespace ChatApplication.Shared.Exceptions.NotFound;

public class UserNotFoundException: NotFoundException
{
    public UserNotFoundException(string id): base($"User with id {id} was not found") {}
}