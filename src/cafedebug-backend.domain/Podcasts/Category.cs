using cafedebug_backend.domain.Shared;
namespace cafedebug_backend.domain.Podcasts;

public class Category : Entity
{
    public string Name { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Category() { }
    public Category(string name)
    {
        Name = name;
        CreatedAt = DateTime.Now;
    }

    public void Update(string name)
    {
        Name = name;
        UpdatedAt = DateTime.Now;
    }
}