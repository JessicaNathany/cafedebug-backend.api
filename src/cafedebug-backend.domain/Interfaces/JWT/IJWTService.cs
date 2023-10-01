namespace cafedebug_backend.domain.Interfaces.JWT
{
    public interface IJWTService
    {
        Task<string> GenerateTokenAsync(string userName, string email);
    }
}
