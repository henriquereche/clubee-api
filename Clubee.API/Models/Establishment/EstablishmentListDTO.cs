using Clubee.API.Contracts.Enums;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Clubee.API.Models.Establishment
{
    public class EstablishmentListDTO
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string ImageThumbnail { get; set; }
        public IEnumerable<EstablishmentTypeEnum> EstablishmentTypes { get; set; }
    }
}
