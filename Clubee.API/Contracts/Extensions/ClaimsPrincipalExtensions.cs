using Clubee.API.Contracts.Infrastructure.Authorization;
using MongoDB.Bson;
using System.Security.Claims;

namespace Clubee.API.Contracts.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Get user id.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static ObjectId GetUserId(this ClaimsPrincipal user) 
            => new ObjectId(user.FindFirst(ClaimsDefaults.Id).Value);

        /// <summary>
        /// Get user email.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetUserEmail(this ClaimsPrincipal user) 
            => user.FindFirst(ClaimsDefaults.Email).Value;

        /// <summary>
        /// Get user establishmentId.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static ObjectId GetEstablishmentId(this ClaimsPrincipal user) 
            => new ObjectId(user.FindFirst(ClaimsDefaults.EstablishmentId).Value);

        /// <summary>
        /// Get user establishmentName.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetEstablishmentName(this ClaimsPrincipal user)
            => user.FindFirst(ClaimsDefaults.EstablishmentName).Value;

        /// <summary>
        /// Get user imageThumbnail.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetUserImageThumbnail(this ClaimsPrincipal user)
            => user.FindFirst(ClaimsDefaults.ImageThumbnail).Value;
    }
}
