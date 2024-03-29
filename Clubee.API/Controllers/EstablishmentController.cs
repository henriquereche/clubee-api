﻿using Clubee.API.Contracts.Extensions;
using Clubee.API.Contracts.Services;
using Clubee.API.Models.Base;
using Clubee.API.Models.Establishment;
using Clubee.API.Models.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;

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
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(ListResult<EstablishmentListDTO>))]
        public IActionResult Get([FromQuery]EstablishmentFilter filter)
        {
            return Ok(this.EstablishmentService.List(filter));
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

        [HttpPut]
        [Authorize]
        [SwaggerOperation("Update existing establishment.")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(EstablishmentFindDTO))]
        public async Task<IActionResult> Put([FromBody] EstablishmentUpdateDTO dto)
        {
            EstablishmentFindDTO updatedEstablishment = await this.EstablishmentService.Update
                (this.User.GetEstablishmentId(), dto);

            if (updatedEstablishment == null)
                return NotFound();

            return Ok(updatedEstablishment);
        }
    }
}