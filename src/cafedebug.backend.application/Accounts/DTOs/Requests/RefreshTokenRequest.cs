using System.ComponentModel.DataAnnotations;

namespace cafedebug.backend.application.Accounts.DTOs.Requests
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
