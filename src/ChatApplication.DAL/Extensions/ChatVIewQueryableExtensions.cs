using ChatApplication.DAL.Entities.Views;
using ChatApplication.Shared.Models;

namespace ChatApplication.DAL.Extensions;

public static class ChatViewQueryableExtensions
{
    public static IQueryable<ChatView> FilterByName(
        this IQueryable<ChatView> chats,
        string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return chats;
        }

        return chats.Where(c => c.Name.ToUpper().Contains(name.ToUpper()));
    }

    public static IQueryable<ChatView> Sort(
        this IQueryable<ChatView> chats,
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

    public static IQueryable<ChatView> Paginate(
        this IQueryable<ChatView> chats,
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