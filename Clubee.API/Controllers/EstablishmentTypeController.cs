using Clubee.API.Contracts.Enums;
using Clubee.API.Contracts.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Clubee.API.Controllers
{
    [ControllerName("establishment-types")]
    public class EstablishmentTypeController : BaseApiController
    {
        [HttpGet]
        public IActionResult Get() => Ok(typeof(EstablishmentTypeEnum).GetEnumDescriptions());
    }
}