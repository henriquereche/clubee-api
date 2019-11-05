using Clubee.API.Contracts.Services;
using Clubee.API.Entities;
using Clubee.API.Infrastructure.Authorization;
using Clubee.API.Models.Base;
using Clubee.API.Models.User;
using System.Collections.Generic;
using System.Security.Claims;

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
