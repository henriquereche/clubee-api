using Clubee.API.Contracts.Extensions;
using Clubee.API.Contracts.Services;
using Clubee.API.Models.Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Clubee.API.Controllers
{
    [ControllerName("events")]
    public class EventController : BaseApiController
    {
        private readonly IEventService EventService;

        public EventController(IEventService eventService)
        {
            this.EventService = eventService;
        }

        [HttpGet]
        [SwaggerOperation("List existing events.")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(IEnumerable<EventListDTO>))]
        public IActionResult Get()
        {
            return Ok(this.EventService.List());
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Find existing event by its identifier.")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(EventFindDTO))]
        public IActionResult GetById([FromRoute] string id)
        {
            EventFindDTO eventFind = this.EventService.Find(
                new ObjectId(id)
            );

            if (eventFind == null)
                return NotFound();

            return Ok(eventFind);
        }

        [HttpPost]
        [Authorize]
        [SwaggerOperation("Create a new event.")]
        [SwaggerResponse((int)HttpStatusCode.Created, type: typeof(EventFindDTO))]
        public async Task<IActionResult> Post([FromBody] EventInsertDTO dto)
        {
            EventFindDTO createdEvent = await this.EventService.Insert(
                this.User.GetEstablishmentId(),
                dto
            );

            return Created(
                $"{this.Request.Path.ToString()}/{createdEvent.Id}",
                createdEvent
            );
        }

        [HttpDelete("{id}")]
        [Authorize]
        [SwaggerOperation("Delete existing event by its identifier.")]
        [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(EventFindDTO))]
        public IActionResult Delete([FromRoute] string id)
        {
            EventFindDTO deletedEvent = this.EventService.Delete(
                this.User.GetEstablishmentId(),
                new ObjectId(id)
            );

            return Ok(deletedEvent);
        }
    }
}