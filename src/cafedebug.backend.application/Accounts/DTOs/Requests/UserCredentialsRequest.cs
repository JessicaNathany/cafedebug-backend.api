namespace cafedebug.backend.application.Accounts.DTOs.Requests
{
    public sealed record UserCredentialsRequest
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
