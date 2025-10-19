namespace cafedebug_backend.domain.Messages.Email.Request;

public class SendEmailRequest
{
    public string Name { get; set; } 
    public string EmailFrom { get; set; }
    public string MessageType { get; set; }
    public string Subject { get; set; }
    public string MessageBody { get; set; }
    public string EmailTo { get; set; } 
    public string EmailCopy { get; set; }
}