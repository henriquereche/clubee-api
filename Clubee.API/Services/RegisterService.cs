using Clubee.API.Contracts.Infrastructure.Data;
using Clubee.API.Contracts.Infrastructure.Storage;
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
        private readonly IObjectStorageProvider ObjectStorageProvider;
        private readonly IImageService ImageService;
        private readonly ILoginService LoginService;

        public RegisterService(
            IMongoRepository mongoRepository,
            IObjectStorageProvider objectStorageProvider,
            IImageService imageService,
            ILoginService loginService
            )
        {
            this.MongoRepository = mongoRepository;
            this.ObjectStorageProvider = objectStorageProvider;
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
            CompressedImageModel compressedImage = this.ImageService.CompressFromBase64(dto.Image);

            string imageUrl = await this.ObjectStorageProvider.SetObject(
                RegisterService.EstablishmentContainer,
                $"{this.GetRandomGuidStringValue()}.{compressedImage.Format}",
                compressedImage.Buffer
            );

            string thumbnailUrl = await this.ObjectStorageProvider.SetObject(
                RegisterService.EstablishmentContainer,
                $"{this.GetRandomGuidStringValue()}.{compressedImage.Format}",
                compressedImage.ThumbnailBuffer
            );

            Establishment establishment = this.CreateEstablishment(imageUrl, thumbnailUrl, dto);
            this.MongoRepository.Insert(establishment);

            User user = this.CreateUser(establishment.Id, dto.User);
            this.MongoRepository.Insert(user);

            return this.LoginService.Login(user);
        }

        /// <summary>
        /// Create establishment from specified dto.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="thumbnailUrl"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        private Establishment CreateEstablishment(string imageUrl, string thumbnailUrl, RegisterEstablishmentDTO dto)
        {
            return new Establishment(
                dto.Name,
                imageUrl,
                thumbnailUrl,
                dto.Description,
                new GeoJson2DGeographicCoordinates(
                    dto.Longitude,
                    dto.Latitude
                ),
                dto.EstablishmentTypes,
                dto.Availabilities.Select(availability =>
                    new Availability(
                        availability.DayOfWeek,
                        availability.OpenTime,
                        availability.CloseTime
                    )
                )
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
            string salt = this.GetRandomGuidStringValue();
            string password = this.LoginService.GeneratePasswordHash(dto.Password, salt);

            return new User(
                establishmentId,
                dto.Email,
                password,
                salt
            );
        }

        /// <summary>
        /// Generates new guid value.
        /// </summary>
        /// <returns></returns>
        private string GetRandomGuidStringValue() 
            => Guid.NewGuid().ToString();
    }
}
