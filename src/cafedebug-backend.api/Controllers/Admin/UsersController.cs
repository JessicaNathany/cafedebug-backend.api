using cafedebug.backend.application.Accounts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Controllers.Admin
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/admin-users")]
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
