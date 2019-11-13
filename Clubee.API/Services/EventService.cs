using Clubee.API.Contracts.Enums;
using Clubee.API.Contracts.Exceptions;
using Clubee.API.Contracts.Infrastructure.Data;
using Clubee.API.Contracts.Services;
using Clubee.API.Entities;
using Clubee.API.Models.Base;
using Clubee.API.Models.Event;
using Clubee.API.Models.Filters;
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
                Image = document["Image"]["Uri"].AsString,
                Description = document["Description"].AsString,
                Location = !document["Location"].IsBsonNull
                    ? new EventFindLocationDTO
                    {
                        City = document["Location"]["City"].AsString,
                        Country = document["Location"]["Country"].AsString,
                        Number = (uint)document["Location"]["Number"].AsInt32,
                        State = document["Location"]["State"].AsString,
                        Street = document["Location"]["Street"].AsString,
                        Longitude = document["Location"]["Coordinates"].AsBsonArray.First().AsDouble,
                        Latitude = document["Location"]["Coordinates"].AsBsonArray.Last().AsDouble
                    }
                    : !document["Establishment"]["Location"].IsBsonNull 
                        ? new EventFindLocationDTO
                        {
                            City = document["Establishment"]["Location"]["City"].AsString,
                            Country = document["Establishment"]["Location"]["Country"].AsString,
                            Number = (uint)document["Establishment"]["Location"]["Number"].AsInt32,
                            State = document["Establishment"]["Location"]["State"].AsString,
                            Street = document["Establishment"]["Location"]["Street"].AsString,
                            Longitude = document["Establishment"]["Location"]["Coordinates"].AsBsonArray.First().AsDouble,
                            Latitude = document["Establishment"]["Location"]["Coordinates"].AsBsonArray.Last().AsDouble
                        }
                        : null,
                Establishment = new EventListEstablishmentDTO
                {
                    Id = document["EstablishmentId"].AsObjectId,
                    Name = document["Establishment"]["Name"].AsString,
                    ImageThumbnail = document["Establishment"]["ImageThumbnail"]["Uri"].AsString,
                }
            };
        }

        /// <summary>
        /// Return event list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EventListDTO> List(EventFilter filter)
        {
            IAggregateFluent<Event> eventAggregateFluent = this.MongoRepository.GetCollection<Event>().Aggregate();

            if (filter.GeospatialQuery)
            {
                BsonDocument geoNearOptions = new BsonDocument {
                    {
                        "near", new BsonDocument {
                            { "type", "Point" },
                            { "coordinates", new BsonArray { filter.Longitude.Value, filter.Latitude.Value} },
                        }
                    },
                    { "maxDistance", filter.Meters ?? 10000 },
                    { "includeLocs", "Location.Coordinates" },
                    { "distanceField", "Location.Distance" },
                    { "spherical" , true }
                };

                eventAggregateFluent = eventAggregateFluent.AppendStage(
                    (PipelineStageDefinition<Event, Event>)new BsonDocument { { "$geoNear", geoNearOptions } }
                );
            }

            if (!string.IsNullOrEmpty(filter.Query))
                eventAggregateFluent = eventAggregateFluent.Match(
                    Builders<Event>.Filter.Text(filter.Query, new TextSearchOptions { CaseSensitive = false, DiacriticSensitive = false, Language = "portuguese" })
                );

            if (!string.IsNullOrEmpty(filter.EstablishmentId))
                eventAggregateFluent = eventAggregateFluent.Match(document => document.EstablishmentId == new ObjectId(filter.EstablishmentId));

            if (filter.Genre.HasValue)
                eventAggregateFluent = eventAggregateFluent.Match(document => document.Genres.Contains(filter.Genre.Value));

            IAggregateFluent<BsonDocument> aggregateFluent = eventAggregateFluent.Lookup(
                nameof(Establishment),
                "EstablishmentId",
                "_id",
                "Establishment"
            ).Unwind("Establishment");

            IEnumerable<BsonDocument> documents = aggregateFluent
                .Skip((filter.Page - 1) * filter.PageSize)
                .Limit(filter.PageSize)
                .ToList();

            return documents.Select(document =>
                new EventListDTO
                {
                    Id = document["_id"].AsObjectId,
                    Name = document["Name"].AsString,
                    StartDate = document["StartDate"].ToUniversalTime(),
                    EndDate = document["EndDate"].ToUniversalTime(),
                    Genres = document["Genres"].AsBsonArray.Select(x => (GenreEnum)x.AsInt32),
                    ImageThumbnail = document["ImageThumbnail"]["Uri"].AsString,
                    Establishment = new EventListEstablishmentDTO
                    {
                        Id = document["EstablishmentId"].AsObjectId,
                        Name = document["Establishment"]["Name"].AsString,
                        ImageThumbnail = document["Establishment"]["ImageThumbnail"]["Uri"].AsString,
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
            Establishment establishment = this.MongoRepository.FindById<Establishment>(establishmentId);

            Event eventEntity = this.CreateEvent(establishment, uploadedImage, dto);

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
        /// Update existing event.
        /// </summary>
        /// <param name="establishmentId"></param>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<EventFindDTO> Update(ObjectId establishmentId, ObjectId id, EventUpdateDTO dto)
        {
            Event updateEvent = this.MongoRepository.FindById<Event>(id);
            if (updateEvent == null || updateEvent.EstablishmentId != establishmentId)
                return null;

            Establishment establishment = this.MongoRepository.FindById<Establishment>(establishmentId);

            updateEvent.Name = dto.Name;
            updateEvent.Description = dto.Description;
            updateEvent.SetDate(dto.StartDate, dto.EndDate);

            if (!string.IsNullOrEmpty(dto.Image))
            {
                await this.ImageService.DeleteImage(updateEvent.Image);
                await this.ImageService.DeleteImage(updateEvent.ImageThumbnail);

                UploadImageModel uploadedImage = await this.ImageService.UploadImage(
                    EventService.EventsContainer, dto.Image);

                updateEvent.Image = uploadedImage.Image;
                updateEvent.ImageThumbnail = uploadedImage.Thumbnail;
            }

            updateEvent.Genres.Clear();
            foreach (GenreEnum genre in dto.Genres)
                updateEvent.AddGenre(genre);

            updateEvent.EstablishmentLocation = dto.Location == null;
            updateEvent.Location = dto.Location != null
                ? new Location(
                    dto.Location.Street,
                    dto.Location.Number,
                    dto.Location.State,
                    dto.Location.Country,
                    dto.Location.City,
                    new GeoJson2DGeographicCoordinates(
                        dto.Location.Longitude,
                        dto.Location.Latitude
                    )
                )
                : establishment.Location;

            this.MongoRepository.Update(updateEvent);
            return this.Find(id);
        }

        /// <summary>
        /// Create Event from specified dto.
        /// </summary>
        /// <param name="establishment"></param>
        /// <param name="uploadedImage"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        private Event CreateEvent(Establishment establishment, UploadImageModel uploadedImage, EventInsertDTO dto)
        {
            return new Event(
                establishment.Id,
                dto.StartDate,
                dto.EndDate,
                dto.Name,
                dto.Description,
                uploadedImage.Image,
                uploadedImage.Thumbnail,
                dto.Location != null
                    ? new Location(
                        dto.Location.Street,
                        dto.Location.Number,
                        dto.Location.State,
                        dto.Location.Country,
                        dto.Location.City,
                        new GeoJson2DGeographicCoordinates(
                            dto.Location.Longitude,
                            dto.Location.Latitude
                        )
                    )
                    : establishment.Location,
                dto.Location == null,
                dto.Genres
            );
        }
    }
}
