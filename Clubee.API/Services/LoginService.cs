using Clubee.API.Contracts.Services;
using Clubee.API.Entities;
using Clubee.API.Infrastructure.Authorization;
using Clubee.API.Models.Base;
using Clubee.API.Models.User;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Clubee.API.Services
{
    public class LoginService : ILoginService
    {
        private JwtAuthorizationTokenWriter JwtAuthorizationTokenWriter;

        public LoginService(JwtAuthorizationTokenWriter jwtAuthorizationTokenWriter)
        {
            this.JwtAuthorizationTokenWriter = jwtAuthorizationTokenWriter;
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
                new Claim("id", user.Id.ToString()),
                new Claim("businessId", user.BusinessId.ToString()),
                new Claim("email", user.Email)
            };
        }
    }
}
