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
                if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                        return Unauthorized("Email and password must not be empty.");

                var userResult =  await _userService.GettByEmailAsync(email, cancellationToken); 

                if(!userResult.IsSuccess)
                    return NotFound("User not found.");

                var user = userResult.Value;

                var token = _jWTService.GenerateToken(user);

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
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(refreshToken))
                return Unauthorized("RefreshToken canot not be nulll.");

            //var refreshTokenResult = await _jWTService.Cre

            return Ok("");
        }
    }
}
