using System.Net;

public class ApiResponse
{
    public bool IsSuccess { get; set; }
    public object Result { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public List<string> ErrorMessages { get; set; } = [];

    public ApiResponse()
    {

    }
}