using cafedebug_backend.api.Validation;
using cafedebug_backend.api.ViewModels;
using cafedebug_backend.domain.Interfaces.JWT;
using cafedebug_backend.domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Administrator.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJWTService _jwtService;
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IJWTService jwtService, IUserService userService, ILogger<AuthController> logger)
        {
            _jwtService = jwtService;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> AuthenticationAsync([FromBody] UserModel model, CancellationToken cancellationToken)
        {
            var validator = new EmailValidator();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                _logger.LogError("Email invalid.");
                return BadRequest(validationResult.Errors);
            }

            var existUser = await _userService.GetByLoginAndPasswordAsync(model.Email, model.Password, cancellationToken);

            if (existUser is null)
            {
                _logger.LogError("Email or Password invalid.");
                return Unauthorized("Email or Password invalid.");
            }

            var tokenResponse = _jwtService.GenerateTokenAsync(model.Email, model.Password);

            if (tokenResponse is null)
                return BadRequest();

            return Ok(tokenResponse);
        }
    }
}
