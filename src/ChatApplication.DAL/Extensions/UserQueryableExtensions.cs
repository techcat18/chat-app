using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
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
                u.FirstName.ToUpper().Contains(searchString.ToUpper()) ||
                u.LastName.ToUpper().Contains(searchString.ToUpper()));
    }
    
    public static IQueryable<User> Sort(this IQueryable<User> users, string? orderByQueryString) 
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
        {
            return users.OrderBy(e => e.Email);
        }

        var orderParams = orderByQueryString.Trim().Split(','); 
        var propertyInfos = typeof(User).GetProperties(BindingFlags.Public | BindingFlags.Instance); 
        var orderQueryBuilder = new StringBuilder(); 
            
        foreach (var param in orderParams) 
        { 
            if (string.IsNullOrWhiteSpace(param)) 
                continue; 
                
            var propertyFromQueryName = param.Split(" ")[0]; 
            var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase)); 
                
            if (objectProperty == null) 
                continue; 
                
            var direction = param.EndsWith(" desc") ? "descending" : "ascending"; 
            orderQueryBuilder.Append($"{objectProperty.Name} {direction}, "); 
        } 
            
        var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
        
        if (string.IsNullOrWhiteSpace(orderQuery))
        {
            return users.OrderBy(e => e.Email); 
        }

        return users.OrderBy(orderQuery); 
    }
}