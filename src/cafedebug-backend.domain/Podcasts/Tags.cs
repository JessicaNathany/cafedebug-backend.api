using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Podcasts
{
    public class Tags : Entity
    {
        public Tags() {}
        public Tags(string name)
        {
            Name = name;
            CreatedAt = DateTime.UtcNow;
        }

        public string Name { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public void Update(string name)
        {
            Name = name;
            UpdatedAt = DateTime.UtcNow;
        }   
    }
}
