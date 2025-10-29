using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Podcasts;

public class Category : Entity
{
    private Category() { }
    public string Name { get; set; }
}