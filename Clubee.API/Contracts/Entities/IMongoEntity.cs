using MongoDB.Bson;

namespace Clubee.API.Contracts.Entities
{
    /// <summary>
    /// Contract to represent mongo document.
    /// </summary>
    public interface IMongoEntity
    {
        /// <summary>
        /// Document identifier.
        /// </summary>
        ObjectId Id { get; }
    }
}
