using System.ComponentModel.DataAnnotations;

namespace cafedebug.backend.application.Request
{
    public class ForgotPasswordRequest
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Name { get; set; }
    }
}
