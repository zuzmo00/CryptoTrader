namespace Crypro.Entities
{
    public class ApiResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; } = true;
        public object? Data { get; set; } = null;
        public int StatusCode { get; set; } = 200;

    }
}
