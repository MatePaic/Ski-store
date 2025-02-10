namespace API.RequestHelpers
{
    public class PaginationMetadata<T>(int totalCount, int pageSize, int pageNumber)
    {
        public int TotalCount { get; set; } = totalCount;
        public int PageSize { get; set; } = pageSize;
        public int CurrentPage { get; set; } = pageNumber;
        public int TotalPages { get; set; } = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}
