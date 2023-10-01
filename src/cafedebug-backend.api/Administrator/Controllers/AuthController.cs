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
    public class AuthController : BaseAdminController
    {
        private readonly IJWTService _jwtService;
        private readonly IUserService _userService;
        public AuthController(IJWTService jwtService, IUserService userService)
        {
            _jwtService = jwtService;
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Authentication([FromBody] UserViewModel model)
        {
            if(String.IsNullOrEmpty(model.Login) && String.IsNullOrEmpty(model.Password))
                return BadRequest(ModelState);

            var validateUser = _userService.GetByLoginAndPasswordAsync(model.Login, model.Password);

            // colocar um validaiton

            var tokenResponse = _jwtService.GenerateTokenAsync(model.Login, model.Email);

            if (tokenResponse == null)
                return BadRequest();

            return Ok(tokenResponse);
        }

    }
}
