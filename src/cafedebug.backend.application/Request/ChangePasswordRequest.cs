using System.ComponentModel.DataAnnotations;

namespace cafedebug.backend.application.Request
{
    public class ChangePasswordRequest
    {
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Email invalid")]
        public string Email { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
