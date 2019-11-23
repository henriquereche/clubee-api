using Clubee.API.Contracts.Extensions;
using Clubee.API.Contracts.Infrastructure.Authorization;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System.Security.Claims;

namespace Clubee.API.Infrastructure.Authorization
{
    public class UserContext : IUserContext
    {
        public UserContext(IHttpContextAccessor contextAccessor)
        {
            ClaimsPrincipal user = contextAccessor.HttpContext.User;
            this.IsAuthenticated = user.Identity.IsAuthenticated;

            if (this.IsAuthenticated)
            {
                this.Id = user.GetUserId();
                this.EstablishmentId = user.GetEstablishmentId();
                this.EstablishmentName = user.GetEstablishmentName();
                this.Email = user.GetEstablishmentName();
                this.ImageThumbnail = user.GetUserImageThumbnail();
            }
        }

        /// <summary>
        /// Indicates user id from request context.
        /// </summary>
        public ObjectId? Id { get; protected set; }

        /// <summary>
        /// Indicates user establishmentId from request context.
        /// </summary>
        public ObjectId? EstablishmentId { get; protected set; }

        /// <summary>
        /// Indicates user email from request context.
        /// </summary>
        public string Email { get; protected set; }

        /// <summary>
        /// Indicates user imageThumbnail from request context.
        /// </summary>
        public string ImageThumbnail { get; protected set; }

        /// <summary>
        /// Indicates user establishmentName from request context.
        /// </summary>
        public string EstablishmentName { get; protected set; }

        /// <summary>
        /// Indicates if user is authenticated.
        /// </summary>
        public bool IsAuthenticated { get; protected set; }
    }
}
