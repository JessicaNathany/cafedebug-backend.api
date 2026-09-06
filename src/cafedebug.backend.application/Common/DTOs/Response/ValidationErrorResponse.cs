namespace cafedebug.backend.application.Common.DTOs.Response;

public sealed record ValidationErrorResponse
{
    public string Code { get; init; }
    public string Message { get; init; }
    public Dictionary<string, string[]> Errors { get; init; }
}
