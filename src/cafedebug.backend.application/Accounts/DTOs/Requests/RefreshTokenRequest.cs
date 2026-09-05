using System.ComponentModel.DataAnnotations;

namespace cafedebug.backend.application.Accounts.DTOs.Requests;

public sealed record RefreshTokenRequest
{
    [Required]
    public required string RefreshToken { get; init; }
}
