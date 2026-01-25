namespace cafedebug.backend.application.Accounts.DTOs.Requests
{
    public class UserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
