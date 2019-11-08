using Clubee.API.Contracts.Enums;
using Clubee.API.Contracts.Exceptions;
using Clubee.API.Contracts.Infrastructure.Data;
using Clubee.API.Contracts.Services;
using Clubee.API.Entities;
using Clubee.API.Models.Base;
using Clubee.API.Models.Establishment;
using Clubee.API.Models.Register;
using MongoDB.Bson;
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

        public EstablishmentService(IMongoRepository mongoRepository, IImageService imageService)
        {
            this.MongoRepository = mongoRepository;
            this.ImageService = imageService;
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
        /// <returns></returns>
        public IEnumerable<EstablishmentListDTO> List()
        {
            return this.MongoRepository.Find<Establishment>(x => true)
                .Select(x => new EstablishmentListDTO
                {
                    Id = x.Id,
                    EstablishmentTypes = x.EstablishmentTypes,
                    ImageThumbnail = x.ImageThumbnail,
                    Name = x.Name
                });
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

            return new EstablishmentFindDTO
            {
                Id = establishment.Id,
                Name = establishment.Name,
                Image = establishment.Image,
                ImageThumbnail = establishment.ImageThumbnail,
                Description = establishment.Description,
                Location = establishment.Address != null
                    ? new EstablishmentFindLocationDTO
                    {
                        Address = establishment.Address,
                        Latitude = establishment.Location.Latitude,
                        Longitude = establishment.Location.Longitude
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
        public EstablishmentFindDTO Update(ObjectId id, EstablishmentUpdateDTO dto)
        {
            Establishment establishment = this.MongoRepository.FindById<Establishment>(id);

            if (establishment == null)
                return null;

            establishment.Name = dto.Name;
            establishment.Description = dto.Description;
            establishment.Address = dto.Location.Address;
            establishment.Location = new GeoJson2DGeographicCoordinates(
                establishment.Location.Longitude,
                establishment.Location.Latitude
            );

            establishment.EstablishmentTypes.Clear();
            foreach (EstablishmentTypeEnum establishmentType in dto.EstablishmentTypes)
                establishment.AddEstablishmentType(establishmentType);

            establishment.Availabilities.Clear();
            foreach (AvailabilityUpdateDTO availability in dto.Availabilities)
                establishment.AddAvailability(
                    new Availability(
                        availability.DayOfWeek,
                        availability.OpenTime, 
                        availability.CloseTime
                    )
                );

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
                uploadedImage.ImageUrl,
                uploadedImage.ThumbnailUrl,
                dto.Description,
                dto.Location != null
                    ? new GeoJson2DGeographicCoordinates(dto.Location.Longitude, dto.Location.Latitude)
                    : null,
                dto.Location?.Address,
                dto.EstablishmentTypes,
                dto.Availabilities != null
                    ? dto.Availabilities.Select(availability => new Availability(availability.DayOfWeek, availability.OpenTime, availability.CloseTime))
                    : null
            );
        }
    }
}
