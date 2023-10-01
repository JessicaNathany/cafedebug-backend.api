using cafedebug_backend.domain.Enums;

namespace cafedebug_backend.domain.Entities
{
    public class User : Entity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public UserType UserType { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}
