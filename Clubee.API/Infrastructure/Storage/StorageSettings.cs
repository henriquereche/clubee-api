using Clubee.API.Contracts.Exceptions;
using Clubee.API.Contracts.Extensions;
using Microsoft.Extensions.Configuration;

namespace Clubee.API.Infrastructure.Storage
{
    /// <summary>
    /// Class to organize application storage settings.
    /// </summary>
    public class StorageSettings
    {
        public readonly string Connection;

        public StorageSettings(IConfiguration configuration)
        {
            IConfigurationSection storageSection = configuration.GetSection("Storage");

            this.Connection = storageSection.TryGetValue("Connection", out string connection)
                ? connection : throw new MissingEnvironmentVariableException("Storage.Connection");
        }
    }
}
