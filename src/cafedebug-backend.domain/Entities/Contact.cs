using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Entities
{
    public class Contact : Entity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime MessageDate { get; set; }
    }
}
