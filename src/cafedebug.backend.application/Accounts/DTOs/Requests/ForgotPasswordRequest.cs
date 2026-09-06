using System.ComponentModel.DataAnnotations;

namespace cafedebug.backend.application.Accounts.DTOs.Requests;

public sealed record ForgotPasswordRequest
{
    [Required]
    [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Email invalid")]
    public required string Email { get; init; }

    [Required]
    public required string Name { get; init; }
}
