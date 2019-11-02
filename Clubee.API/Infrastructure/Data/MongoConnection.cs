using Clubee.API.Contracts.Exceptions;
using Clubee.API.Contracts.Extensions;
using Clubee.API.Contracts.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;

namespace Clubee.API.Infrastructure.Data
{
    public class MongoConnection : IMongoConnection
    {
        private readonly IConfiguration Configuration;
        private readonly ILogger<MongoConnection> Logger;

        public MongoConnection(
            IConfiguration configuration,
            ILogger<MongoConnection> logger
            )
        {
            this.Configuration = configuration;
            this.Logger = logger;

            this.Initialize();
        }

        /// <summary>
        /// Initializes connection.
        /// </summary>
        private void Initialize()
        {
            try
            {
                IConfigurationSection mongoSettings = this.Configuration.GetSection("MongoSettings");

                MongoClientSettings clientSettings = MongoClientSettings.FromConnectionString(
                    mongoSettings.TryGetValue("Connection", out string connection)
                        ? connection : throw new MissingEnvironmentVariableException("MongoSettings.Connection")
                );

                this.Client = new MongoClient(clientSettings);

                this.Database = this.Client.GetDatabase(
                    mongoSettings.TryGetValue("Database", out string database)
                        ? database : throw new MissingEnvironmentVariableException("MongoSettings.Database")
                );
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, "Failed to start MongoDB connection.");
                throw;
            }
        }

        /// <summary>
        /// Mongo application client.
        /// </summary>
        public IMongoClient Client { get; private set; }

        /// <summary>
        /// Mongo database.
        /// </summary>
        public IMongoDatabase Database { get; private set; }
    }
}
