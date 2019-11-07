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
        public StorageSettings(IConfiguration configuration)
        {
            IConfigurationSection storageSection = configuration.GetSection("Storage");

            this.Account = storageSection.TryGetValue("Account", out string account)
                ? account : throw new MissingEnvironmentVariableException("Storage.Account");

            this.Key = storageSection.TryGetValue("Key", out string key)
                ? key : throw new MissingEnvironmentVariableException("Storage.Key");

            this.Address = storageSection.TryGetValue("Address", out string address)
                ? address : throw new MissingEnvironmentVariableException("Storage.Address");

            this.Port = storageSection.TryGetValue("Port", out int port)
                ? port : throw new MissingEnvironmentVariableException("Storage.Port");

            this.Protocol = storageSection.TryGetValue("Protocol", out string protocol)
                ? protocol : throw new MissingEnvironmentVariableException("Storage.Protocol");

            this.EndpointSuffix = storageSection.TryGetValue("EndpointSuffix", out string endpointSuffix)
                ? endpointSuffix : default;
        }

        /// <summary>
        /// Storage account.
        /// </summary>
        public string Account { get; private set; }

        /// <summary>
        /// Storage key.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Storage address.
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        /// Storage port.
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Storage default protocol.
        /// </summary>
        public string Protocol { get; private set; }

        /// <summary>
        /// Storage endpoint suffix.
        /// </summary>
        public string EndpointSuffix { get; set; }

        /// <summary>
        /// Storage endpoint.
        /// </summary>
        public string Endpoint => $"{this.Protocol}://{this.Address}:{this.Port}/{this.Account}";

        /// <summary>
        /// Get storage connection string.
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            return $"DefaultEndpointsProtocol={this.Protocol};"
                + $"AccountName={this.Account};"
                + $"AccountKey={this.Key};"
                + this.EndpointSuffix != default 
                    ? $"EndpointSuffix={this.EndpointSuffix}" 
                    : $"BlobEndpoint={this.Endpoint};";
        }
    }
}
