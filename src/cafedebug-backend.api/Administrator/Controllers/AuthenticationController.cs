using cafedebug.backend.application.Request;
using cafedebug.backend.application.Service;
using cafedebug_backend.domain.Interfaces.JWT;
using cafedebug_backend.domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Administrator.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJWTService _jWTService;
        public AuthenticationController(IUserService userService, IJWTService jWTService)
        {
            _userService = userService;
            _jWTService = jWTService;
        }

        [HttpPost]
        [Route("Auth")]
        public async Task<IActionResult> Login([FromBody] string email, string password, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                    return Unauthorized("Email and password must not be empty.");

                var userResult = await _userService.GettByEmailAsync(email, cancellationToken);

                if (!userResult.IsSuccess)
                    return NotFound("User not found.");

                var user = userResult.Value;

                var token = await _jWTService.GenerateToken(user, cancellationToken);

                return Ok(token);
            }

            catch (NullReferenceException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (string.IsNullOrEmpty(refreshTokenRequest.Token))
                return Unauthorized("Refresh token cannot be null.");

            var refreshToken = await _jWTService.GetByTokenAsync(refreshTokenRequest.Token, cancellationToken);

            if (refreshToken == null || !refreshToken.IsActive || refreshToken.ExpirationDate <= DateTime.UtcNow)
                return Unauthorized("Invalid or expired refresh token.");

            var user = await _userService.GetByIdAsync(refreshToken.UserId, cancellationToken);

            if (user is null)
                return NotFound("User not found.");

            var newAcessToken = await _jWTService.GenerateToken(user, cancellationToken);

            await _jWTService.SaveRefreshTokenAsync(newAcessToken.RefreshToken, cancellationToken);

            return Ok(newAcessToken.RefreshToken);
        }
    }
}
