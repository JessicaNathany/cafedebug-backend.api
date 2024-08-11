using cafedebug.backend.application.Request;
using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Jwt;

namespace cafedebug.backend.api.test.Mocks
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
                userName: "debugcafe@local.com", 
                token: "fake-refresh-token", 
                expirationDate: DateTime.Now.AddMinutes(15));
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
               Token = "fake-refresh",
               UserId = 1
            };
        }
    }
}
