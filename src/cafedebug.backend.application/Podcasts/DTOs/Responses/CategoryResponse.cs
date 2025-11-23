namespace cafedebug.backend.application.Podcasts.DTOs.Responses;

public sealed record CategoryResponse 
{
    public int Id { get; set; }
    public string Name { get; set; }
}