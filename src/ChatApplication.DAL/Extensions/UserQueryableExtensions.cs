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

    public static IQueryable<User> FilterBySearchString(
        this IQueryable<User> users,
        string? searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
        {
            return users;
        }

        return users
            .Where(u => 
                u.Email.ToUpper().Contains(searchString.ToUpper()) ||
                u.UserName.ToUpper().Contains(searchString.ToUpper()));
    }
}