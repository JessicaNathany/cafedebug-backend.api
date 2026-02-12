using cafedebug_backend.domain.Shared;
namespace cafedebug_backend.domain.Podcasts;

public class Category : Entity
{
    public Category() { }
    public string Name { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
}