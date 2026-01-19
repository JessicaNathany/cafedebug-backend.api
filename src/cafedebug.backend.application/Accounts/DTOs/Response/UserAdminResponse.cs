namespace cafedebug.backend.application.Accounts.DTOs.Response
{
    public class UserAdminResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string HashedPassword { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
