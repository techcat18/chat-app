namespace ChatApplication.Shared.Models;

public class ChatFilterModel: PaginationModel
{
    public string? SearchString { get; set; }
    public string? SortingOption { get; set; }
}