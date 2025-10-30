using cafedebug.backend.application.Common.Mappings;
using cafedebug_backend.domain.Podcasts;

namespace cafedebug.backend.application.Podcasts.DTOs.Responses;

public sealed record CategoryResponse : IMapFrom<Category>
{
    public Guid Code { get; set; }
    public string Name { get; set; }
}