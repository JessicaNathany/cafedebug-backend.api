using System.ComponentModel.DataAnnotations;

namespace cafedebug.backend.application.Accounts.DTOs.Requests;

public sealed record ChangePasswordRequest
{
    [Required]
    [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Email invalid")]
    public required string Email { get; init; }

    [Required]
    public required string NewPassword { get; init; }
}
