namespace ProductCatalog.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; }
        public string? Message { get; }
        public T? Data { get; }

        public ApiResponse(bool success, T? data = default, string? message = null)
        {
            Success = success;
            Data = data;
            Message = message;
        }
        public static ApiResponse<T> Ok(T data, string? message = null) =>
            new ApiResponse<T>(true, data, message);

        public static ApiResponse<T> Fail(string message) =>
            new ApiResponse<T>(false, default, message);
    }
}
