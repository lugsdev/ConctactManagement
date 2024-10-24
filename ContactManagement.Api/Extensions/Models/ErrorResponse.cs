namespace ContactManagement.Api.Extensions.Models
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
