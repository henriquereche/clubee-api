using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
