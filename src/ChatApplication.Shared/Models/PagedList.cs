using System.Text.Json.Serialization;

namespace ChatApplication.Shared.Models;

public class PagedList<T>
{
    [JsonPropertyName("data")]
    public IEnumerable<T> Data { get; set; }
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }
    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    public PagedList()
    {
        
    }

    private PagedList(IEnumerable<T> data, int totalCount, int currentPage, int pageSize)
    {
        Data = data;
        TotalCount = totalCount;
        CurrentPage = currentPage;

        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
    
    public static PagedList<T> ToPagedModel(IEnumerable<T> data, int totalCount, int currentPage, int pageSize)
    {
        return new PagedList<T>(data, totalCount, currentPage, pageSize);
    }
}