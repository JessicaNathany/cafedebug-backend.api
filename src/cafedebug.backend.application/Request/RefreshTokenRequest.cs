using System.ComponentModel.DataAnnotations;

namespace cafedebug.backend.application.Request
{
    public class RefreshTokenRequest
    {
        [Required]
        public string Token { get; set; }

        public int UserId { get; set; }
        
    }
}
