using Clubee.API.Contracts.Enums;
using Clubee.API.Contracts.Extensions;
using Clubee.API.Models.Base;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;

namespace Clubee.API.Controllers
{
    [ControllerName("establishment-types")]
    public class EstablishmentTypeController : BaseApiController
    {
        [HttpGet]
        [SwaggerOperation("List establishment types.")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<EnumDescriptionModel>))]
        public IActionResult Get() => Ok(typeof(EstablishmentTypeEnum).GetEnumDescriptions());
    }
}