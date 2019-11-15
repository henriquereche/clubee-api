using Clubee.API.Contracts.Enums;
using Clubee.API.Contracts.Extensions;
using Clubee.API.Contracts.Infrastructure.Data;
using Clubee.API.Contracts.Infrastructure.Telemetry;
using Clubee.API.Contracts.Services;
using Clubee.API.Entities;
using Clubee.API.Models.Base;
using Clubee.API.Models.Establishment;
using Clubee.API.Models.Filters;
using Clubee.API.Models.Register;
using Microsoft.ApplicationInsights;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clubee.API.Services
{
    public class EstablishmentService : IEstablishmentService
    {
        public const string EstablishmentContainer = "establishments";

        private readonly IMongoRepository MongoRepository;
        private readonly IImageService ImageService;
        private readonly TelemetryClient TelemetryClient;

        public EstablishmentService(
            IMongoRepository mongoRepository, 
            IImageService imageService,
            TelemetryClient telemetryClient)
        {
            this.MongoRepository = mongoRepository;
            this.ImageService = imageService;
            this.TelemetryClient = telemetryClient;
        }

        /// <summary>
        /// Inserts a new establishment.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Establishment> Insert(RegisterEstablishmentDTO dto)
        {
            UploadImageModel uploadedImage = await this.ImageService.UploadImage(
               EstablishmentService.EstablishmentContainer, dto.Image);

            Establishment establishment = this.CreateEstablishment(uploadedImage, dto);
            this.MongoRepository.Insert(establishment);

            return establishment;
        }

        /// <summary>
        /// Return establishment list.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<EstablishmentListDTO> List(EstablishmentFilter filter)
        {
            IAggregateFluent<Establishment> establishmentAggregateFluent = this.MongoRepository
                .GetCollection<Establishment>().Aggregate();

            if (filter.GeospatialQuery)
            {
                BsonDocument geoNearOptions = new BsonDocument {
                    {
                        "near", new BsonDocument {
                            { "type", "Point" },
                            { "coordinates", new BsonArray { filter.Longitude.Value, filter.Latitude.Value } },
                        }
                    },
                    { "maxDistance", filter.Meters ?? 10000 },
                    { "includeLocs", "Location.Coordinates" },
                    { "distanceField", "Location.Distance" },
                    { "spherical" , true }
                };

                establishmentAggregateFluent = establishmentAggregateFluent.AppendStage(
                    (PipelineStageDefinition<Establishment, Establishment>)new BsonDocument { { "$geoNear", geoNearOptions } }
                );

                this.TelemetryClient.TrackEvent(
                    EventNames.EstablishmentListLocation,
                    new { filter.Latitude, filter.Longitude }
                );
            }

            if (filter.EstablishmentType.HasValue)
            {
                establishmentAggregateFluent = establishmentAggregateFluent
                    .Match(document => document.EstablishmentTypes.Contains(filter.EstablishmentType.Value));

                this.TelemetryClient.TrackEvent(
                    EventNames.EstablishmentListType,
                    new { filter.EstablishmentType }
                );
            }

            if (!string.IsNullOrEmpty(filter.Query))
            {
                establishmentAggregateFluent = establishmentAggregateFluent.Match(
                    document => document.Name.Contains(filter.Query)
                        || document.Description.Contains(filter.Query)
                );

                this.TelemetryClient.TrackEvent(
                    EventNames.EstablishmentListQuery,
                    new { filter.Query }
                );
            }

            IEnumerable<BsonDocument> documents = establishmentAggregateFluent
                .Skip((filter.Page - 1) * filter.PageSize)
                .Limit(filter.PageSize)
                .As<BsonDocument>()
                .ToList();

            return documents.Select(document => 
                new EstablishmentListDTO
                {
                    Id = document["_id"].AsObjectId,
                    EstablishmentTypes = document["EstablishmentTypes"].AsBsonArray.Select(x => (EstablishmentTypeEnum)x.AsInt32),
                    ImageThumbnail = document["ImageThumbnail"]["Uri"].AsString,
                    Name = document["Name"].AsString,
                    Distance = document["Location"].AsBsonDocument.Contains("Distance")
                        ? document["Location"]["Distance"].AsDouble
                        : (double?)null
                }
            ).ToList();
        }

        /// <summary>
        /// Find establishment by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EstablishmentFindDTO Find(ObjectId id)
        {
            Establishment establishment = this.MongoRepository.FindById<Establishment>(id);

            if (establishment == null)
                return null;

            this.TelemetryClient.TrackEvent(
                EventNames.EstablishmentFind,
                new { id }
            );

            return new EstablishmentFindDTO
            {
                Id = establishment.Id,
                Name = establishment.Name,
                Image = establishment.Image.Uri,
                Description = establishment.Description,
                Location = establishment.Location != null
                    ? new EstablishmentFindLocationDTO
                    {
                        Street = establishment.Location.Street,
                        City = establishment.Location.City,
                        Country = establishment.Location.Country,
                        Number = establishment.Location.Number,
                        State = establishment.Location.State,
                        Latitude = establishment.Location.Coordinates.Latitude,
                        Longitude = establishment.Location.Coordinates.Longitude
                    }
                    : null,
                EstablishmentTypes = establishment.EstablishmentTypes,
                Availabilities = establishment.Availabilities.Select(availability => new EstablishmentAvailabilityFindDTO
                {
                    CloseTime = availability.CloseTime,
                    DayOfWeek = availability.DayOfWeek,
                    OpenTime = availability.OpenTime,
                    Duration = availability.Duration
                })
            };
        }

        /// <summary>
        /// Update existing establishment.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<EstablishmentFindDTO> Update(ObjectId id, EstablishmentUpdateDTO dto)
        {
            Establishment establishment = this.MongoRepository.FindById<Establishment>(id);

            if (establishment == null)
                return null;

            establishment.Name = dto.Name;
            establishment.Description = dto.Description;

            if (dto.Location != null)
            {
                establishment.Location = new Location(
                    dto.Location.Street,
                    dto.Location.Number,
                    dto.Location.State,
                    dto.Location.Country,
                    dto.Location.City,
                    new GeoJson2DGeographicCoordinates(
                        dto.Location.Longitude,
                        dto.Location.Latitude
                    )
                );

                IEnumerable<Event> establishmentEvents = this.MongoRepository
                    .Find<Event>(x => x.EstablishmentLocation);

                foreach (Event establishmentEvent in establishmentEvents)
                {
                    establishmentEvent.Location = establishment.Location;
                    this.MongoRepository.Update(establishmentEvent);
                }
            }
                
            if (!string.IsNullOrEmpty(dto.Image))
            {
                await this.ImageService.DeleteImage(establishment.Image);
                await this.ImageService.DeleteImage(establishment.ImageThumbnail);

                UploadImageModel uploadedImage = await this.ImageService.UploadImage(
                    EstablishmentService.EstablishmentContainer, dto.Image);

                establishment.Image = uploadedImage.Image;
                establishment.ImageThumbnail = uploadedImage.Thumbnail;
            }

            establishment.EstablishmentTypes.Clear();
            foreach (EstablishmentTypeEnum establishmentType in dto.EstablishmentTypes)
                establishment.AddEstablishmentType(establishmentType);

            if (dto.Availabilities != null)
            {
                establishment.Availabilities.Clear();
                foreach (AvailabilityUpdateDTO availability in dto.Availabilities)
                    establishment.AddAvailability(
                        new Availability(
                            availability.DayOfWeek,
                            availability.OpenTime,
                            availability.CloseTime
                        )
                    );
            }

            this.MongoRepository.Update(establishment);
            return this.Find(id);
        }

        /// <summary>
        /// Create establishment from specified dto.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="thumbnailUrl"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        private Establishment CreateEstablishment(UploadImageModel uploadedImage, RegisterEstablishmentDTO dto)
        {
            return new Establishment(
                dto.Name,
                uploadedImage.Image,
                uploadedImage.Thumbnail,
                dto.Description,
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
                dto.EstablishmentTypes,
                dto.Availabilities != null
                    ? dto.Availabilities.Select(availability => new Availability(availability.DayOfWeek, availability.OpenTime, availability.CloseTime))
                    : null
            );
        }
    }
}
