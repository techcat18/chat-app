using Microsoft.AspNetCore.Identity;

namespace ChatApplication.DAL.Entities;

public class User: IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public ICollection<UserChat> UserChats { get; set; }
}