using Clubee.API.Contracts.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Clubee.API.Infrastructure.Authorization
{
    public static class JwtAuthorizationServerConfigurations
    {
        /// <summary>
        /// Add Jwt authorization to application.
        /// </summary>
        /// <param name="services"></param>
        public static void AddJwtAuthorizarion(this IServiceCollection services)
        {
            services.AddSingleton<JwtAuthorizationSettings>();
            services.AddSingleton<JwtAuthorizationTokenWriter>();
            services.AddScoped<IUserContext, UserContext>();

            JwtAuthorizationSettings jwtAuthorizationSettings = services.BuildServiceProvider()
                .GetService<JwtAuthorizationSettings>();

            byte[] key = Encoding.ASCII.GetBytes(jwtAuthorizationSettings.SecretKey);

            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = false;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }
}
