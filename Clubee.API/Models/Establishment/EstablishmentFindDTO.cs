using Clubee.API.Contracts.Enums;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Clubee.API.Models.Establishment
{
    public class EstablishmentFindDTO
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string ImageThumbnail { get; set; }
        public string Description { get; set; }
        public EstablishmentFindLocationDTO Location { get; set; }
        public IEnumerable<EstablishmentTypeEnum> EstablishmentTypes { get; set; }
        public IEnumerable<EstablishmentAvailabilityFindDTO> Availabilities { get; set; }
    }
}
