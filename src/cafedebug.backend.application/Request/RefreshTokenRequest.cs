namespace cafedebug.backend.application.Request
{
    public class RefreshTokenRequest
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string ExpirationDate { get; set; }
    }
}
