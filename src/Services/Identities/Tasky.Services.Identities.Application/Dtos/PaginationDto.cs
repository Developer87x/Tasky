namespace Tasky.Services.Identities.Application.Dtos
{
    public class PaginationDto<T>
    {
        public List<T> Items { get; set; } =[];
        public int  TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => Page < TotalPages;
        public bool HasPreviousPage => Page > 1;

    }

    public class PaginationRequestDto
    {
        private const int MaxPageSize = 100;
        private int _pageSize = 10;
        public int Page { get; set; } = 1;
        public int PageSize 
        { 
            get => _pageSize; 
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value; 
        }
        public int Offset => (Page - 1) * PageSize;
    }
}
