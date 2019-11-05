using Clubee.API.Contracts.Services;
using Clubee.API.Models.Establishment;
using Clubee.API.Models.User;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Clubee.API.Controllers
{
    [ControllerName("register")]
    public class RegisterController : BaseApiController
    {
        private readonly IRegisterService RegisterService;

        public RegisterController(IRegisterService registerService)
        {
            this.RegisterService = registerService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterEstablishmentDTO dto)
        {
            UserLoginResultDTO response = await this.RegisterService.Register(dto);
            return Ok(response);
        }
    }
}