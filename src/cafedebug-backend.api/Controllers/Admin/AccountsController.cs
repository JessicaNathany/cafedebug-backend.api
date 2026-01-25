using cafedebug.backend.application.Accounts.DTOs.Requests;
using cafedebug.backend.application.Accounts.Interfaces;
using cafedebug_backend.domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Controllers.Admin
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/accounts-admin")]
    public class AccountsController(IUserService userService, IAccountService accountService) : ControllerBase
    {
        // Notes: We need to include in FluentValidation the if (!ModelState.IsValid) in all the endpoints
        // and status code related to each endpoint.

        [HttpPost]
        [Route("forgot-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> ForgotPassword([FromBody] ForgotPasswordRequest emailRequest)
        {
            return await accountService.ForgotPassword(emailRequest);
        }

        [HttpPost]
        [Authorize]
        [Route("change-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            return await accountService.ChangePassword(request);
        }

        [HttpPost]
        [Authorize]
        [Route("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> ResetPassword([FromBody] ChangePasswordRequest request)
        {
            return await accountService.ResetPassword(request);
        }

        [HttpPost]
        [Authorize]
        [Route("verify-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> VerifyEmail([FromBody] string email)
        {
            throw new NotImplementedException();
        }
    }
}
