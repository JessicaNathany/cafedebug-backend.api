using cafedebug.backend.application.Request;
using cafedebug_backend.domain.Interfaces.JWT;
using cafedebug_backend.domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Administrator.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJWTService _jWTService;
        public AuthController(IUserService userService, IJWTService jWTService)
        {
            _userService = userService;
            _jWTService = jWTService;
        }

        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> GetToken([FromBody] UserCredentialsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                    return Unauthorized("Email and password must not be empty.");

                if(!ModelState.IsValid)
                {
                    // add log here
                    return BadRequest("Model is invalid.");
                }

                var userResult = await _userService.GettByEmailAsync(request.Email,request.Password, cancellationToken);

                if (!userResult.IsSuccess)
                    return Unauthorized("User not found.");

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
