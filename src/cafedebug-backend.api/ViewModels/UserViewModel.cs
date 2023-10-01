using cafedebug_backend.domain.Enums;

namespace cafedebug_backend.api.ViewModels
{
    public class UserViewModel
    {
        public string Name { get; set; }

        public UserType UserType { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
    }
}
