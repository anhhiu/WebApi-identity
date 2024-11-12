namespace WebApi.Common
{
    public class ServiceResponse<T> where T : class
    {
        public T? Data { get; set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }

    }
}
