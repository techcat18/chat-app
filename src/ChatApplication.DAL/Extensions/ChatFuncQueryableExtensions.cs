using ChatApplication.DAL.Entities.Functions;
using ChatApplication.Shared.Models;

namespace ChatApplication.DAL.Extensions;

public static class ChatFuncQueryableExtensions
{
    public static IQueryable<ChatFunction> FilterByName(
        this IQueryable<ChatFunction> chats,
        string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return chats;
        }

        return chats.Where(c => c.Name.ToUpper().Contains(name.ToUpper()));
    }

    public static IQueryable<ChatFunction> Sort(
        this IQueryable<ChatFunction> chats,
        string? sortingOption)
    {
        if (string.IsNullOrWhiteSpace(sortingOption))
        {
            return chats;
        }
        
        return sortingOption switch
        {
            ChatSortingOptions.Name => chats.OrderBy(c => c.Name),
            _ => chats
        };
    }

    public static IQueryable<ChatFunction> Paginate(
        this IQueryable<ChatFunction> chats,
        int page,
        int count)
    {
        if (page <= 0 || count <= 0)
        {
            return chats;
        }

        return chats
            .Skip(count * (page - 1))
            .Take(count);
    }
}