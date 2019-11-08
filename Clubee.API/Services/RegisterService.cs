using Clubee.API.Contracts.Exceptions;
using Clubee.API.Contracts.Infrastructure.Data;
using Clubee.API.Contracts.Services;
using Clubee.API.Entities;
using Clubee.API.Models.Base;
using Clubee.API.Models.Establishment;
using Clubee.API.Models.User;
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Clubee.API.Services
{
    public class RegisterService : IRegisterService
    {
        public const string EstablishmentContainer = "establishments";

        private readonly IMongoRepository MongoRepository;
        private readonly IImageService ImageService;
        private readonly ILoginService LoginService;

        public RegisterService(
            IMongoRepository mongoRepository,
            IImageService imageService,
            ILoginService loginService
            )
        {
            this.MongoRepository = mongoRepository;
            this.ImageService = imageService;
            this.LoginService = loginService;
        }

        /// <summary>
        /// Register a new user and establishment.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<UserLoginResultDTO> Register(RegisterEstablishmentDTO dto)
        {
            dto.User.Email = dto.User.Email.ToLower();

            if (this.MongoRepository.Exists<User>(x => x.Email == dto.User.Email))
                throw new BadRequestException($"Email {dto.User.Email} already in use.");

            UploadImageModel uploadedImage = await this.ImageService.UploadImage(
                RegisterService.EstablishmentContainer, dto.Image);
            
            Establishment establishment = this.CreateEstablishment(uploadedImage, dto);
            this.MongoRepository.Insert(establishment);

            User user = this.CreateUser(establishment.Id, dto.User);
            this.MongoRepository.Insert(user);

            return this.LoginService.Login(user, establishment);
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

        /// <summary>
        /// Create user from specified dto.
        /// </summary>
        /// <param name="establishmentId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        private User CreateUser(ObjectId establishmentId, RegisterUserDTO dto)
        {
            string salt = Guid.NewGuid().ToString();
            string password = this.LoginService.GeneratePasswordHash(dto.Password, salt);

            return new User(
                establishmentId,
                dto.Email,
                password,
                salt
            );
        }
    }
}
