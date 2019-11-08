using MongoDB.Bson;

namespace Clubee.API.Models.Event
{
    public class EventListEstablishmentDTO
    {
        public ObjectId Id { get; set; }
        public string Name{ get; set; }
        public string ImageThumbnail { get; set; }
    }
}
