using Clubee.API.Contracts.Infrastructure.Data;
using Clubee.API.Contracts.Infrastructure.Storage;
using Clubee.API.Infrastructure.Data;
using Clubee.API.Infrastructure.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Clubee.API.Infrastructure.DI
{
    public static class DependencyInjectionProvider
    {
        /// <summary>
        /// Register all application services on DI.
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<StorageSettings>();
            services.AddScoped<IObjectStorageProvider, ObjectStorageProvider>();

            services.AddScoped<IMongoConnection, MongoConnection>();
            services.AddScoped<IMongoRepository, MongoRepository>();
        }
    }
}
