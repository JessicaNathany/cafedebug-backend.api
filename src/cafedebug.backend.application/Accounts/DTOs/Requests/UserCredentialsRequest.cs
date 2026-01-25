using System.ComponentModel.DataAnnotations;

namespace cafedebug.backend.application.Accounts.DTOs.Requests
{
    public class UserCredentialsRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
