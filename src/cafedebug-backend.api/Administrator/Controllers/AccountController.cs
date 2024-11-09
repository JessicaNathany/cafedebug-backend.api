using cafedebug.backend.application.Constants;
using cafedebug.backend.application.Request;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Administrator.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/account")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AuthController> logger, IUserService userService, IAccountService accountService)
        {
            _userService = userService;
            _logger = logger;
            _accountService = accountService;
        }

        [HttpPost]
        [Route("forgot-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest emailRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation("Model is invalid.");
                    return BadRequest("Model is invalid.");
                }

                var user = _userService.GetUserAdminByEmail(emailRequest.Email);

                if (user.Result.Value is null)
                {
                    _logger.LogInformation("User not found.");
                    return NotFound("User not found.");
                }

                var sendEmail = new SendEmailRequest
                {
                    Name = emailRequest.Name,
                    Email = emailRequest.Email,
                    Subject = "Reset Password",
                    MessageType = InsfrastructureConstants.MessageTypeResetPassword,
                };

                await _accountService.SendEmailForgotPassword(sendEmail);

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("change-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation("Model is invalid.");
                    return BadRequest("Model is invalid.");
                }

                return NoContent();
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        [Route("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ResetPassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation("Model is invalid.");
                    return BadRequest("Model is invalid.");
                }

                return NoContent();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("verify-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> VerifyEmail([FromBody] ChangePasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation("Model is invalid.");
                    return BadRequest("Model is invalid.");
                }

                return NoContent();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
