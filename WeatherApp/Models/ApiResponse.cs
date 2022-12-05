using System.Text.Json.Serialization;

namespace WeatherApp.Models
{
    public class ApiResponse<T>
    {
        public string Status { get; set; } = default!;
        
        public string? Error { get; set; } = default!;
        public string Message { get; set; } = default!;
        public T? Data { get; set; } = default!;


        public static ApiResponse<T> SuccessResponse (T data, string message = "")
        {
            return new ApiResponse<T>
            {
                Data = data,
                Message = message,
                Status = "ok"
            };
        }

        public static ApiResponse<T> ErrorResponse (string error, string message = "")
        {
            return new ApiResponse<T>
            {
                Message = message,
                Error = error,
                Status = "error"
            };
        }
    }
}
