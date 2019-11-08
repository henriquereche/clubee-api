using Clubee.API.Contracts.Enums;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Clubee.API.Models.Event
{
    public class EventFindDTO
    {
        public ObjectId Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public IEnumerable<GenreEnum> Genres { get; set; }
        public EventFindLocationDTO Location { get; set; }
        public EventListEstablishmentDTO Establishment { get; set; }
    }
}
