using cafedebug.backend.application.Accounts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Controllers.Admin;
[ApiController]
[Produces("application/json")]
[Route("api/v1/admin-users")]
public class UsersController(IJWTService jwtService, IUserService userService, ILogger<AuthController> logger)
    : ControllerBase
{

    private readonly IJWTService _jwtService = jwtService;
    private readonly IUserService _userService = userService;
    private readonly ILogger<AuthController> _logger = logger;
}
