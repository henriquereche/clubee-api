using Clubee.API.Contracts.Enums;
using Clubee.API.Contracts.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Clubee.API.Controllers
{
    [ControllerName("genres")]
    public class GenreController : BaseApiController
    {
        [HttpGet]
        public IActionResult Get() => Ok(typeof(GenreEnum).GetEnumDescriptions());
    }
}