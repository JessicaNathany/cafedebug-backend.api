namespace cafedebug_backend.domain.Interfaces.JWT
{
    public interface IJWTService
    {
        string GenerateTokenAsync(string userName, string email);
    }
}
