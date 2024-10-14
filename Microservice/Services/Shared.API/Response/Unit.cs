namespace Shared.API.Response
{
    public class Unit
    {
        public Unit(int statusCode, bool isSuccess, string message)
        {
            StatusCode = statusCode;
            IsSuccess = isSuccess;
            Message = message;
        }

        public Unit()
        {
            
        }

        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
