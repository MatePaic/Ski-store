namespace API.Errors
{
    public class ApiErrorResponse(int statusCode, string title, string? details)
    {
        public int StatusCode { get; set; } = statusCode;
        public string Title { get; set; } = title;
        public string? Details { get; set; } = details;
    }
}
