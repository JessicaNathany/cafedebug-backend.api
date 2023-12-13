namespace cafedebug_backend.domain.Entities
{
    public class Entity 
    {
        public int Id { get; set; }

        public Guid Code = Guid.NewGuid();
    }
}
