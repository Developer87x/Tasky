namespace Tasky.BuildingBlocks.Core.Pagination;

public class PageList<T>(IReadOnlyList<T> items, int count, int pageNumber, int pageSize) : IPageList<T> where T : class
{
    public int CurrentPage { get; private set; } = pageNumber;
    public int TotalPages { get; private set; } = (int)Math.Ceiling(count / (double)pageSize);
    public int PageSize { get; private set; } = pageSize;
    public int TotalCount { get; private set; } = count;
    public IReadOnlyList<T> Items { get; private set; } = items;
}