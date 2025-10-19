namespace cafedebug_backend.domain.Shared;

public class Entity 
{
    public int Id { get; set; }

    public Guid Code = Guid.NewGuid();
}