namespace cafedebug.backend.application.Common.Constants;

public static class JWTConstants
{
    public static string JwtIssuer => Environment.GetEnvironmentVariable("Issuer") ?? "DefaultIssuer";
    public static string JwtAudience => Environment.GetEnvironmentVariable("Audience") ?? "DefaultAudience";

    public static string JWTSecret = Environment.GetEnvironmentVariable("SecretKey");
}