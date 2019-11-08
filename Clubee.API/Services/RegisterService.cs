using Clubee.API.Contracts.Exceptions;
using Clubee.API.Contracts.Infrastructure.Data;
using Clubee.API.Contracts.Services;
using Clubee.API.Entities;
using Clubee.API.Models.Register;
using Clubee.API.Models.User;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace Clubee.API.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IMongoRepository MongoRepository;
        private readonly IEstablishmentService EstablishmentService;
        private readonly ILoginService LoginService;

        public RegisterService(
            IMongoRepository mongoRepository,
            IEstablishmentService establishmentService,
            ILoginService loginService
            )
        {
            this.MongoRepository = mongoRepository;
            this.EstablishmentService = establishmentService;
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

            Establishment establishment = await this.EstablishmentService.Insert(dto);

            User user = this.CreateUser(establishment.Id, dto.User);
            this.MongoRepository.Insert(user);

            return this.LoginService.Login(user, establishment);
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
