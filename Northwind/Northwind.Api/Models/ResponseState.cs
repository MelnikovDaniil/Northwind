namespace Northwind.Api.Models
{
    public class ResponseState
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public ResponseState()
        {
            IsSuccess = true;
        }

        public ResponseState(string message)
        {
            IsSuccess = false;
            Message = message;
        }
    }
}
