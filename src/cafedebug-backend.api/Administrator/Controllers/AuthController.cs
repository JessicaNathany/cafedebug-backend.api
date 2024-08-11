using cafedebug.backend.application.Request;
using cafedebug_backend.domain.Interfaces.JWT;
using cafedebug_backend.domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Administrator.Controllers
{
    [ApiController]
    [Authorize]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetToken([FromBody] UserCredentialsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                    return BadRequest("Email and password must not be empty.");

                if(!ModelState.IsValid)
                {
                    // add log here
                    return BadRequest("Model is invalid.");
                }

                var userResult = await _userService.GetByEmailAsync(request.Email,request.Password, cancellationToken);

                if (!userResult.IsSuccess)
                    return Unauthorized("User not found.");

                var user = userResult.Value;
                var token = await _jWTService.GenerateToken(user);

                if (token is null)
                    return Unauthorized("User unauthorized.");

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                if (string.IsNullOrEmpty(refreshTokenRequest.Token))
                        return Unauthorized("Refresh token cannot be null.");

                var refreshToken = await _jWTService.GetByTokenAsync(refreshTokenRequest.Token);

                if (refreshToken == null || !refreshToken.Value.IsActive || refreshToken.Value.ExpirationDate <= DateTime.UtcNow)
                    return Unauthorized("Invalid or expired refresh token.");

                var user = await _userService.GetByIdAsync(refreshToken.Value.UserId, cancellationToken);

                if (user is null)
                    return NotFound("User not found.");

                var newAcessToken = await _jWTService.GenerateToken(user);

                if (newAcessToken is null)
                    return Unauthorized("User unauthorized.");

                return Ok(newAcessToken.RefreshToken);
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
    }
}
