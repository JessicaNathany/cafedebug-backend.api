using cafedebug_backend.domain.Accounts.Services;
using cafedebug.backend.application.Accounts.DTOs.Requests;
using cafedebug.backend.application.Accounts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Controllers.Admin
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/admin-auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;
        private readonly IJWTService _jWTService;
        public AuthController(ILogger<AuthController> logger, IUserService userService, IJWTService jWTService)
        {
            _logger = logger;
            _userService = userService;
            _jWTService = jWTService;
        }

        [HttpPost]
        [Route("token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GenerateToken([FromBody] UserCredentialsRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                    return BadRequest("Email and password must not be empty.");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Login attempt with invalid model state.");
                    return BadRequest("Model is invalid.");
                }

                var userResult = await _userService.GetByEmailAsync(request.Email, request.Password);

                if (!userResult.IsSuccess)
                {
                    _logger.LogWarning("Login attempt failed for the provided email.");
                    return Unauthorized("User not found or invalid credentials.");
                }

                var user = userResult.Value;
                var token = await _jWTService.GenerateAccesTokenAndRefreshtoken(user);

                if (token is null)
                {
                    _logger.LogError("Error creating token for user.");
                    return BadRequest("Error creating token for user.");
                }

                return Ok(token);
            }
            catch (UnauthorizedAccessException)
            {
                _logger.LogWarning("Unauthorized access during authentication.");
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in authentication endpoint.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state in refresh token request.");
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrEmpty(refreshTokenRequest.RefreshToken))
                {
                    _logger.LogWarning("Refresh token missing in request.");
                    return Unauthorized("Refresh token cannot be null.");
                }

                var refreshTokenResult = await _jWTService.GetByTokenAsync(refreshTokenRequest.RefreshToken);

                if (refreshTokenResult.Value == null || refreshTokenResult.Value.ExpirationDate <= DateTime.UtcNow)
                {
                    _logger.LogWarning("Invalid or expired refresh token.");
                    return Unauthorized("Invalid or expired refresh token.");
                }

                var userResult = await _userService.GetByIdAsync(refreshTokenResult.Value.UserId);

                if (!userResult.IsSuccess)
                {
                    _logger.LogWarning("User not found for refresh token.");
                    return NotFound("User not found.");
                }

                var token = await _jWTService.RefreshTokenAsync(refreshTokenResult.Value, userResult.Value);

                if (token is null)
                {
                    _logger.LogError("Error creating new token during refresh.");
                    return BadRequest("Error creating token.");
                }

                return Ok(token);
            }
            catch (UnauthorizedAccessException)
            {
                _logger.LogWarning("Unauthorized access during refresh token.");
                return Unauthorized();
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "NullReferenceException in refresh token endpoint.");
                return StatusCode(500, "Internal server error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in refresh token endpoint.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
