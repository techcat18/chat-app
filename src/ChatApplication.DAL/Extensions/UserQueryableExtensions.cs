using ChatApplication.DAL.Entities;

namespace ChatApplication.DAL.Extensions;

public static class UserQueryableExtensions
{
    public static IQueryable<User> Paginate(
        this IQueryable<User> users,
        int page,
        int count)
    {
        if (page <= 0 || count <= 0)
        {
            return users;
        }

        return users
            .Skip((page - 1) * count)
            .Take(count);
    }

    public static IQueryable<User> FilterByEmail(
        this IQueryable<User> users,
        string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return users;
        }

        return users
            .Where(u => u.Email.ToUpper().Contains(email.ToUpper()));
    }
    
    public static IQueryable<User> FilterByUserName(
        this IQueryable<User> users,
        string? userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            return users;
        }

        return users
            .Where(u => u.UserName.ToUpper().Contains(userName.ToUpper()));
    }
}