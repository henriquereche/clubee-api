using Clubee.API.Contracts.Services;
using Clubee.API.Models.User;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Clubee.API.Controllers
{
    [ControllerName("login")]
    public class LoginController : BaseApiController
    {
        private readonly ILoginService LoginService;

        public LoginController(ILoginService loginService)
        {
            this.LoginService = loginService;
        }

        [HttpPost]
        [SwaggerOperation("Users login.")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(UserLoginResultDTO))]
        public IActionResult Login([FromBody] UserLoginDTO dto)
        {
            UserLoginResultDTO response = this.LoginService.Login(dto);
            return Ok(response);
        }
    }
}