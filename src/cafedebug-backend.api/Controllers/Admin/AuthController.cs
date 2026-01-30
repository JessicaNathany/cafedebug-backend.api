using cafedebug.backend.application.Accounts.DTOs.Requests;
using cafedebug.backend.application.Accounts.Interfaces;
using cafedebug_backend.domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Controllers.Admin
{
    /// <summary>
    /// Manages authentication for administrators
    /// </summary>
    /// <remarks>
    /// This controller provides JWT token generation and refresh operations for the system.
    /// </remarks>
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/admin/auth")]
    [Tags("Admin - Auth")]
    public class AuthController(IJWTService jWTService) : ControllerBase
    {
        [HttpPost]
        [Route("token")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> GenerateToken([FromBody] UserCredentialsRequest request)
        {
            return await jWTService.GenerateToken(request.Email, request.Password);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("refresh-token")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Result>> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            return await jWTService.RefreshTokenAsync(refreshTokenRequest.RefreshToken);
        }
    }
}
