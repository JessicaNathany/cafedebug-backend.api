using System.ComponentModel.DataAnnotations;

namespace cafedebug.backend.application.Request
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
