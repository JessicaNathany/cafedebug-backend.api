namespace cafedebug.backend.application.Common.DTOs.Response;

public class ValidationErrorResponse
{
    public string Code { get; set; }
    public string Message { get; set; }
    public Dictionary<string, string[]> Errors { get; set; }
}