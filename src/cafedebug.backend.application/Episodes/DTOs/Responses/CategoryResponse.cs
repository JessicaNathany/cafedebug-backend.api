using cafedebug_backend.domain.Episodes.Entities;
using cafedebug.backend.application.Common.Mappings;

namespace cafedebug.backend.application.Episodes.DTOs.Responses;

public sealed record CategoryResponse : IMapFrom<Category>
{
    public Guid Code { get; set; }
    public string Name { get; set; }
}