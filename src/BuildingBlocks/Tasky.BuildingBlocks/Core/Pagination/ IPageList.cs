namespace Tasky.BuildingBlocks.Core.Pagination;

public interface  IPageList<out T> where T:class
{
    int CurrentPage { get; }
    int TotalPages { get; }
    int PageSize { get; }
    int TotalCount { get; }
    bool HasPrevious => CurrentPage > 1;
    bool HasNext => CurrentPage < TotalPages;
    IReadOnlyList<T> Items { get; }
}
