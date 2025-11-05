using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Accounts.Tokens;
using cafedebug.backend.application.Accounts.DTOs.Requests;

namespace cafedebug.backend.api.test.Shared.Mocks
{
    public static class DataMocks
    {
        public static UserAdmin UserAdminMock()
        {
            return new UserAdmin
            {
                Id = 1,
                Code = new Guid("f1d8dcf4-98b6-4e7b-8b8b-7f6f7a3f6c7b"),
                Email = "debugcafe@local.com",
                Name = "Test User",
                HashedPassword = "cf8676b53315b632ec681f2065d6e3c993c3ebaeb667338658b40983d7ce663e"
            };
        }

        public static RefreshTokens RefreshTokenMock()
        {
            return new RefreshTokens(
                userId: 1,
                userName: "debugcafe@local.com", 
                token: "fake-refresh-token", 
                expirationDate: DateTime.Now.AddMinutes(15), DateTime.Now);
        }

        public static UserCredentialsRequest UserRequest()
        {
            return new UserCredentialsRequest
            {
                Email = "debugcafe@local.com",
                Password = "cafedebug123"
            };
        }

        public static RefreshTokenRequest RefreshTokenRequest()
        {
            return new RefreshTokenRequest
            {
               RefreshToken = "fake-refresh",
            };
        }
    }
}
