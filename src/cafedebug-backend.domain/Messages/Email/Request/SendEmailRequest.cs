namespace cafedebug_backend.domain.Messages.Email.Request;

public sealed record SendEmailRequest
{
    public string Name { get; init; }
    public string EmailFrom { get; init; }
    public string MessageType { get; init; }
    public string Subject { get; init; }
    public string MessageBody { get; init; }
    public string EmailTo { get; init; }
    public string EmailCopy { get; init; }
}
