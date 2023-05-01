using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using ChatApplication.DAL.Views;

namespace ChatApplication.DAL.Extensions;

public static class UserViewQueryableExtensions
{
    public static IQueryable<UserView> Paginate(
        this IQueryable<UserView> users,
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

    public static IQueryable<UserView> FilterBySearchString(
        this IQueryable<UserView> users,
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
    
    public static IQueryable<UserView> Sort(this IQueryable<UserView> users, string? orderByQueryString) 
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
        {
            return users.OrderBy(e => e.Email);
        }

        var orderParams = orderByQueryString.Trim().Split(','); 
        var propertyInfos = typeof(UserView).GetProperties(BindingFlags.Public | BindingFlags.Instance); 
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