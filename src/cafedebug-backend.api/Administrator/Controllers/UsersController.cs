using cafedebug_backend.api.Validation;
using cafedebug_backend.api.ViewModels;
using cafedebug_backend.domain.Interfaces.JWT;
using cafedebug_backend.domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ILogger<AuthenticationController> _logger;
        public UsersController(IJWTService jwtService, IUserService userService, ILogger<AuthenticationController> logger)
        {
            _jwtService = jwtService;
            _userService = userService;
            _logger = logger;
        }
    }
}
