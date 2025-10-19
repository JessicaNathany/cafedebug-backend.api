using cafedebug.backend.application.Constants;
using cafedebug.backend.application.Request;
using cafedebug_backend.domain.Interfaces.JWT;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Administrator.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Area(nameof(Administrator))]
    [Route("api/v1/accounts-admin")]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;
        private readonly IJWTService _jWTService;

        public AccountsController(
            ILogger<AuthController> logger, 
            IUserService userService, 
            IAccountService accountService,
            IJWTService jWTService)
        {
            _userService = userService;
            _logger = logger;
            _accountService = accountService;
            _jWTService = jWTService;
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
                    EmailFrom = emailRequest.Email,
                    Subject = "Reset Password",
                    MessageType = InsfrastructureConstants.EmailMessageTypeResetPassword,
                };

                var resetToken = _jWTService.GenerateResetToken(user.Result.Value.Id);

                var urlResetPassword = InsfrastructureConstants.ForgotPasswordUrl;

                var resetUrl = $"{urlResetPassword}?token={resetToken}";

                await _accountService.SendEmailForgotPassword(sendEmail);

                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        [Authorize]
        [Route("change-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
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

                await _accountService.ChangePassword(request.Email, request.NewPassword);

                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
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
        [Authorize]
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

                await _accountService.ResetPassword(request.Email, request.NewPassword);

                return NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Authorize]
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
