namespace Talabat.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);//if message were null do this
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "BadRequest",
                401 => "You Are Not Authorized",
                403=> "the client doesn't have permission to access the requested resource",
                404 => "Recourse Not Found",
                500 => "Internal Server Error",
                _ => null //in not any one of this return null its like defaul

            };

        }
    }
}
