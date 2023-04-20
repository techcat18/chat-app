namespace ChatApplication.Shared.Models;

public class PagedList<T>
{
    public IEnumerable<T> Data { get; private set; }
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int TotalCount { get; private set;  }

    public PagedList(IEnumerable<T> data, int totalCount, int currentPage, int pageSize)
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