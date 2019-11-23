using MongoDB.Bson;

namespace Clubee.API.Contracts.Infrastructure.Authorization
{
    public interface IUserContext
    {
        /// <summary>
        /// Indicates user id from request context.
        /// </summary>
        ObjectId? Id { get; }

        /// <summary>
        /// Indicates user establishmentId from request context.
        /// </summary>
        ObjectId? EstablishmentId { get; }

        /// <summary>
        /// Indicates user email from request context.
        /// </summary>
        string Email { get; }

        /// <summary>
        /// Indicates user imageThumbnail from request context.
        /// </summary>
        string ImageThumbnail { get; }

        /// <summary>
        /// Indicates user establishmentName from request context.
        /// </summary>
        string EstablishmentName { get; }

        /// <summary>
        /// Indicates if user is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }
    }
}
