using Clubee.API.Contracts.Services;
using Clubee.API.Models.Establishment;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;

namespace Clubee.API.Controllers
{
    [ControllerName("establishments")]
    public class EstablishmentController : BaseApiController
    {
        private readonly IEstablishmentService EstablishmentService;

        public EstablishmentController(IEstablishmentService establishmentService)
        {
            this.EstablishmentService = establishmentService;
        }

        [HttpGet]
        [SwaggerOperation("List existing establishments.")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(IEnumerable<EstablishmentListDTO>))]
        public IActionResult Get()
        {
            return Ok(this.EstablishmentService.List());
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Find existing establishment by its identifier.")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(EstablishmentFindDTO))]
        public IActionResult GetById([FromRoute] string id)
        {
            EstablishmentFindDTO establishmentFind = this.EstablishmentService.Find(
                new ObjectId(id)
            );

            if (establishmentFind == null)
                return NotFound();

            return Ok(establishmentFind);
        }
    }
}