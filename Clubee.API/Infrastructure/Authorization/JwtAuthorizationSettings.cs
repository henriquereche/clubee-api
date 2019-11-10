using Clubee.API.Contracts.Exceptions;
using Clubee.API.Contracts.Extensions;
using Microsoft.Extensions.Configuration;
using System;

namespace Clubee.API.Infrastructure.Authorization
{
    /// <summary>
    /// Default class to handle Jwt authorizarion settings.
    /// </summary>
    public class JwtAuthorizationSettings
    {
        public JwtAuthorizationSettings(IConfiguration configuration)
        {
            IConfigurationSection jwtAuthorizationSection = configuration.GetSection("JwtSettings");

            this.SecretKey = jwtAuthorizationSection.TryGetValue("SecretKey", out string secretKey)
                ? secretKey : throw new MissingEnvironmentVariableException("JwtSettings.SecretKey");

            this.Expiration = jwtAuthorizationSection.TryGetValue("ExpirationSeconds", out int expirationSeconds)
                ? TimeSpan.FromSeconds(expirationSeconds)
                : TimeSpan.FromSeconds(86400);
        }

        /// <summary>
        /// Jwt secret key.
        /// </summary>
        public string SecretKey { get; private set; }

        /// <summary>
        /// Jwt token expiration time.
        /// </summary>
        public TimeSpan Expiration { get; private set; }
    }
}
