namespace ChatApplication.Shared.Models;

public class UserFilterModel: PaginationModel
{
    public string? Email { get; set; }
    public string? UserName { get; set; }
}