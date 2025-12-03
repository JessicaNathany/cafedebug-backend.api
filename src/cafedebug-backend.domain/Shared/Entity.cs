namespace cafedebug_backend.domain.Shared;

public class Entity 
{
    public int Id { get; set; }

    // to remove in future
    public Guid Code = Guid.NewGuid();
}