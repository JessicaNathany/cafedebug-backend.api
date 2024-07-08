using cafedebug_backend.domain.Interfaces.JWT;
using cafedebug_backend.domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Administrator.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/user")]
    public class UsersController : ControllerBase
    {

        private readonly IJWTService _jwtService;
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;
        public UsersController(IJWTService jwtService, IUserService userService, ILogger<AuthController> logger)
        {
            _jwtService = jwtService;
            _userService = userService;
            _logger = logger;
        }
    }
}
