namespace ProductCatalog.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse(bool success, T? data = default, string? message = null)
        {
            Success = success;
            Data = data;
            Message = message;
        }
        public static ApiResponse<T> Ok(T data, string? message = null) =>
            new ApiResponse<T>(success: true, message: message, data: data);

        public static ApiResponse<T> Fail(string message) =>
            new ApiResponse<T>(false, data: default, message: message);
    }
}
