namespace Tasky.Services.Identities.Application.Dtos;

public class Pagination<T>
{
    public List<T> Items { get; set; } =[];
    public int  TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;

}
