using cafedebug_backend.domain.Episodes;
using cafedebug_backend.domain.Podcasts;
using cafedebug.backend.application.Common.Mappings;

namespace cafedebug.backend.application.Podcasts.DTOs.Responses;

public sealed record CategoryResponse : IMapFrom<Category>
{
    public Guid Code { get; set; }
    public string Name { get; set; }
}