﻿using Clubee.API.Contracts.Enums;
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
                    : null,
                dto.Genres
            );
        }
    }
}
