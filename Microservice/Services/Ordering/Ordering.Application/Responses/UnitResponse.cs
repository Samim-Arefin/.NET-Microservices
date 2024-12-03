namespace Ordering.Application.Responses
{
    public class UnitResponse
    {
        public UnitResponse(int statusCode, bool isSuccess, string message)
        {
            StatusCode = statusCode;
            IsSuccess = isSuccess;
            Message = message;
        }

        public UnitResponse()
        {

        }

        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
