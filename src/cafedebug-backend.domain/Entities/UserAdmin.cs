namespace cafedebug_backend.domain.Entities
{
    /// <summary>
    /// New entity user application
    /// </summary>
    public class UserAdmin : Entity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string HashedPassword { get; set; }

        public string RefreshToken { get; set; }
    }
}
