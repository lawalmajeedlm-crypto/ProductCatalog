namespace ProductCatalog.DTOs
{
    public class APiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public APiResponse(bool success, T? data = default, string? message = null)
        {
            Success = success;
            Data = data;
            Message = message;
        }
        public static APiResponse<T> Ok(T data, string? message = null) =>
            new APiResponse<T>(success: true, message: message, data: data);

        public static APiResponse<T> Fail(String message) =>
            new APiResponse<T>(false, message: message, data: default);
    }
}
