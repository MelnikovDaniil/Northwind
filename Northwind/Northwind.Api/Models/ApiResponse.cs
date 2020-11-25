namespace Northwind.Api.Models
{
    public class ApiResponse
    {
        public object Data { get; set; }

        public ResponseState ResponseState { get; set; }

        public ApiResponse()
        {
            ResponseState = new ResponseState();
        }

        public ApiResponse(string message)
        {
            ResponseState = new ResponseState(message);
        }
    }
}
