namespace ChatApplication.Shared.Models;

public class UserFilterModel: PaginationModel
{
    public string? SearchString { get; set; }
}