namespace cafedebug_backend.domian.Jwt
{
    public class RefreshToken
    {
        public string UserName { get; set; }    
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }

        public RefreshToken(string userName, string token, DateTime expirationDate)
        {
            UserName = userName;
            Token = token;
            ExpirationDate = expirationDate;
        }

        public static RefreshToken Create(string userName, string token, DateTime expirationDate)
        {
            return new RefreshToken(userName, token, expirationDate);
        }
    }
}
