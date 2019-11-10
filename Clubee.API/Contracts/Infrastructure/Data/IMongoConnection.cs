using MongoDB.Driver;

namespace Clubee.API.Contracts.Infrastructure.Data
{
    public interface IMongoConnection
    {
        /// <summary>
        /// Mongo application client.
        /// </summary>
        IMongoClient Client { get; }

        /// <summary>
        /// Mongo database.
        /// </summary>
        IMongoDatabase Database { get; }
    }
}
