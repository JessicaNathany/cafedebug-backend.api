namespace cafedebug.backend.api.test.Services
{
    public class JWTServiceTest
    {
        [Fact]
        public void GenerateResetToken_ShouldReturnValidJwt()
        {
            // Arrange
            var jwtSettings = JwtSettingsMock.GetValidJwtSettings();
            var loggerMock = new Mock<ILogger<JWTService>>();
            var refreshTokensRepoMock = new Mock<IRefreshTokensRepository>();
            var jwtService = new JWTService(jwtSettings, loggerMock.Object, refreshTokensRepoMock.Object);

            // Act
            var token = jwtService.GenerateResetToken(123);

            // Assert
            Assert.False(string.IsNullOrEmpty(token));
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            Assert.Equal("123", jwt.Payload["userId"]);
            Assert.True(jwt.ValidTo > DateTime.UtcNow);
        }
    }
}
