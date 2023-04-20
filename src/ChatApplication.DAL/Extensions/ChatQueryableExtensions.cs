using ChatApplication.DAL.Entities;
using ChatApplication.Shared.Models;

namespace ChatApplication.DAL.Extensions;

public static class ChatQueryableExtensions
{
    public static IQueryable<Chat> FilterByName(
        this IQueryable<Chat> chats,
        string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return chats;
        }

        return chats.Where(c => c.Name.ToUpper().Contains(name.ToUpper()));
    }

    public static IQueryable<Chat> Sort(
        this IQueryable<Chat> chats,
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

    public static IQueryable<Chat> Paginate(
        this IQueryable<Chat> chats,
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