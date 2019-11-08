using Clubee.API.Contracts.Exceptions;
using Clubee.API.Contracts.Infrastructure.Authorization;
using Clubee.API.Contracts.Infrastructure.Data;
using Clubee.API.Contracts.Services;
using Clubee.API.Entities;
using Clubee.API.Infrastructure.Authorization;
using Clubee.API.Models.Base;
using Clubee.API.Models.User;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Clubee.API.Services
{
    public class LoginService : ILoginService
    {
        private JwtAuthorizationTokenWriter JwtAuthorizationTokenWriter;
        private IMongoRepository MongoRepository;

        public LoginService(
            JwtAuthorizationTokenWriter jwtAuthorizationTokenWriter,
            IMongoRepository mongoRepository
            )
        {
            this.JwtAuthorizationTokenWriter = jwtAuthorizationTokenWriter;
            this.MongoRepository = mongoRepository;
        }

        /// <summary>
        /// Generate access token for the specified user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserLoginResultDTO Login(User user)
        {
            IEnumerable<Claim> claims = this.CreateClaims(user);
            JwtAuthorizationTokenModel token = this.JwtAuthorizationTokenWriter.WriteToken(claims);

            return new UserLoginResultDTO
            {
                Token = token.Token,
                Expiration = token.Expiration
            };
        }

        /// <summary>
        /// Generate access token for the specified user.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public UserLoginResultDTO Login(UserLoginDTO dto)
        {
            NotFoundException error = new NotFoundException("User not found.");
            User user = this.MongoRepository.Find<User>(x => x.Email == dto.Email).FirstOrDefault();

            if (user == null) throw error;
            string password = this.GeneratePasswordHash(dto.Password, user.Salt);

            if (password != user.Password) throw error;
            return this.Login(user);
        }

        /// <summary>
        /// Generate hash from password and salt.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public string GeneratePasswordHash(string password, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password + salt);

            using (SHA512 sha = SHA512.Create())
            {
                byte[] computedHash = sha.ComputeHash(bytes);
                StringBuilder computedString = new StringBuilder();

                foreach (var computedByte in computedHash)
                    computedString.Append(computedByte.ToString("X2"));

                return computedString.ToString();
            }
        }

        /// <summary>
        /// Generate default claims from user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private IEnumerable<Claim> CreateClaims(User user)
        {
            return new Claim[]
            {
                new Claim(ClaimsDefaults.Id, user.Id.ToString()),
                new Claim(ClaimsDefaults.EstablishmentId, user.EstablishmentId.ToString()),
                new Claim(ClaimsDefaults.Email, user.Email)
            };
        }
    }
}
