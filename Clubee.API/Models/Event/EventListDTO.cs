using Clubee.API.Contracts.Enums;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Clubee.API.Models.Event
{
    public class EventListDTO
    {
        public ObjectId Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public string ImageThumbnail { get; set; }
        public double? Distance { get; set; }
        public IEnumerable<GenreEnum> Genres { get; set; }
        public EventListEstablishmentDTO Establishment { get; set; }
    }
}
