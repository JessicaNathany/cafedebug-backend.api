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
        private readonly IConfiguration _configuration;
        public AuthenticationController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Auth")]
        public async Task<IActionResult> Auth([FromBody] string apiKey, string email, CancellationToken cancellationToken)
        {
            try
            {
                // aqui será feito a validação da api key e email para terar um token

                var apiSettings = _configuration.GetValue<string>("ApiSettings:ApiKey");

                var user = _userService.GettByEmailAsync(email, cancellationToken);

                if (user is null)
                    return Unauthorized();

                if (apiKey != apiSettings)
                    return Unauthorized();

                    return Ok("token");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
