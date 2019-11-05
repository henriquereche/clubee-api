using Clubee.API.Contracts.Enums;
using Clubee.API.Contracts.Extensions;
using Clubee.API.Models.Base;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;

namespace Clubee.API.Controllers
{
    [ControllerName("genres")]
    public class GenreController : BaseApiController
    {
        [HttpGet]
        [SwaggerOperation("List genres.")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<EnumDescriptionModel>))]
        public IActionResult Get() => Ok(typeof(GenreEnum).GetEnumDescriptions());
    }
}