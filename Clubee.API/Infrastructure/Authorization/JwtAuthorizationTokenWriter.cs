using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Clubee.API.Infrastructure.Authorization
{
    public class JwtAuthorizationTokenWriter
    {
        private readonly JwtAuthorizationSettings JwtAuthorizationSettings;

        public JwtAuthorizationTokenWriter(JwtAuthorizationSettings jwtAuthorizationSettings)
        {
            this.JwtAuthorizationSettings = jwtAuthorizationSettings;
        }

        /// <summary>
        /// Write a new Jwt token using default settings.
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public string WriteToken(IEnumerable<Claim> claims)
        {
            byte[] key = Encoding.ASCII.GetBytes(this.JwtAuthorizationSettings.SecretKey);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(this.JwtAuthorizationSettings.Expiration),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
