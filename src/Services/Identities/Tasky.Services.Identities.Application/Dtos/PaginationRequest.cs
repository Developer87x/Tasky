namespace Tasky.Services.Identities.Application.Dtos;

public class PaginationRequest
{
    private int _page = 1;
    private const int MaxPageSize = 100;
    private int _pageSize = 10;
    public int Page { get=> _page; set => _page = (value < 1) ? 1 : value; }

    public int PageSize 
    { 
        get => _pageSize; 
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value; 
    }
    public int Offset => (Page - 1) * PageSize;
}
