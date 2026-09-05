using cafedebug.backend.application.Accounts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Controllers.Admin;
[ApiController]
[Produces("application/json")]
[Route("api/v1/admin-users")]
public class UsersController
    : ControllerBase
{
}
