using Clubee.API.Contracts.Infrastructure.Data;
using Clubee.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Clubee.API.Controllers
{
    [ControllerName("establishments")]
    public class EstablishmentController : BaseApiController
    {
        private readonly IMongoRepository MongoRepository;

        public EstablishmentController(IMongoRepository mongoRepository)
        {
            this.MongoRepository = mongoRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.MongoRepository.Find<Establishment>(x => true));
        }
    }
}