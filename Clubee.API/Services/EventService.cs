using Clubee.API.Contracts.Enums;
using Clubee.API.Contracts.Exceptions;
using Clubee.API.Contracts.Infrastructure.Data;
using Clubee.API.Contracts.Services;
using Clubee.API.Entities;
using Clubee.API.Models.Base;
using Clubee.API.Models.Event;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clubee.API.Services
{
    public class EventService : IEventService
    {
        public const string EventsContainer = "events";

        private readonly IMongoRepository MongoRepository;
        private readonly IImageService ImageService;

        public EventService(
            IMongoRepository mongoRepository,
            IImageService imageService
            )
        {
            this.MongoRepository = mongoRepository;
            this.ImageService = imageService;
        }

        /// <summary>
        /// Find event by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EventFindDTO Find(ObjectId id)
        {
            IMongoCollection<Event> eventCollection = this.MongoRepository.GetCollection<Event>();

            BsonDocument document = eventCollection.Aggregate()
                .Match(x => x.Id == id)
                .Lookup(
                    nameof(Establishment),
                    "EstablishmentId",
                    "_id",
                    "Establishment"
                ).Unwind("Establishment")
                .FirstOrDefault();

            if (document == null)
                return null;

            return new EventFindDTO
            {
                Id = document["_id"].AsObjectId,
                Name = document["Name"].AsString,
                StartDate = document["StartDate"].ToUniversalTime(),
                EndDate = document["EndDate"].ToUniversalTime(),
                Genres = document["Genres"].AsBsonArray.Select(x => (GenreEnum)x.AsInt32),
                Image = document["Image"].AsString,
                Description = document["Description"].AsString,
                Location = !document["Address"].IsBsonNull
                    ? new EventFindLocationDTO
                    {
                        Address = document["Address"].AsString,
                        Longitude = document["Location"].AsBsonArray.First().AsDouble,
                        Latitude = document["Location"].AsBsonArray.Last().AsDouble
                    }
                    : document["Establishment"]["Address"].IsBsonNull 
                        ? null
                        : new EventFindLocationDTO
                        {
                            Address = document["Establishment"]["Address"].AsString,
                            Longitude = document["Establishment"]["Location"].AsBsonArray.First().AsDouble,
                            Latitude = document["Establishment"]["Location"].AsBsonArray.Last().AsDouble
                        },
                Establishment = new EventListEstablishmentDTO
                {
                    Id = document["EstablishmentId"].AsObjectId,
                    Name = document["Establishment"]["Name"].AsString,
                    ImageThumbnail = document["Establishment"]["ImageThumbnail"].AsString,
                }
            };
        }

        /// <summary>
        /// Return event list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EventListDTO> List()
        {
            IMongoCollection<Event> eventCollection = this.MongoRepository.GetCollection<Event>();

            IEnumerable<BsonDocument> documents = eventCollection.Aggregate().Lookup(
                nameof(Establishment),
                "EstablishmentId",
                "_id",
                "Establishment"
            ).Unwind("Establishment")
            .ToList();

            return documents.Select(document =>
                new EventListDTO
                {
                    Id = document["_id"].AsObjectId,
                    Name = document["Name"].AsString,
                    StartDate = document["StartDate"].ToUniversalTime(),
                    EndDate = document["EndDate"].ToUniversalTime(),
                    Genres = document["Genres"].AsBsonArray.Select(x => (GenreEnum)x.AsInt32),
                    ImageThumbnail = document["ImageThumbnail"].AsString,
                    Establishment = new EventListEstablishmentDTO
                    {
                        Id = document["EstablishmentId"].AsObjectId,
                        Name = document["Establishment"]["Name"].AsString,
                        ImageThumbnail = document["Establishment"]["ImageThumbnail"].AsString,
                    }
                }
            ).ToList();
        }


        /// <summary>
        /// Inserts a new Event.
        /// </summary>
        /// <param name="establishmentId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<EventFindDTO> Insert(ObjectId establishmentId, EventInsertDTO dto)
        {
            UploadImageModel uploadedImage = await this.ImageService.UploadImage(EventService.EventsContainer, dto.Image);
            Event eventEntity = this.CreateEvent(establishmentId, uploadedImage, dto);

            this.MongoRepository.Insert(eventEntity);
            return this.Find(eventEntity.Id);
        }

        /// <summary>
        /// Delete existing event.
        /// </summary>
        /// <param name="establishmentId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public EventFindDTO Delete(ObjectId establishmentId, ObjectId id)
        {
            EventFindDTO deleteEvent = this.Find(id);

            if (deleteEvent == null)
                throw new NotFoundException($"Event {id} not found.");

            if (deleteEvent.Establishment.Id != establishmentId)
                throw new UnauthorizedException();

            this.MongoRepository.Remove<Event>(id);
            return deleteEvent;
        }

        /// <summary>
        /// Create Event from specified dto.
        /// </summary>
        /// <param name="establishmentId"></param>
        /// <param name="uploadedImage"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        private Event CreateEvent(ObjectId establishmentId, UploadImageModel uploadedImage, EventInsertDTO dto)
        {
            return new Event(
                establishmentId,
                dto.StartDate,
                dto.EndDate,
                dto.Name,
                dto.Description,
                uploadedImage.ImageUrl,
                uploadedImage.ThumbnailUrl,
                dto.Location != null
                    ? new GeoJson2DGeographicCoordinates(dto.Location.Longitude, dto.Location.Latitude)
                    : null,
                dto.Location?.Address,
                dto.Genres
            );
        }
    }
}
