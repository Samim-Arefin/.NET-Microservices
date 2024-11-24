namespace Shared.API.Response
{
    public class ErrorResponse
    {
        public ErrorResponse()
        {
            
        }

        public ErrorResponse(int statusCode, string title, string message)
        {
            StatusCode = statusCode;
            Title = title;
            Message = message;
        }

        public int StatusCode { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
