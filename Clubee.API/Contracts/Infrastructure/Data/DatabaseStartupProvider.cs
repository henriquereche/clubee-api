using Clubee.API.Entities;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Clubee.API.Contracts.Infrastructure.Data
{
    public static class DatabaseStartupProvider
    {
        /// <summary>
        /// Initialize application collections indexes.
        /// </summary>
        /// <param name="services"></param>
        public static void InitializeCollectionsIndexes(this IServiceCollection services)
        {
            IMongoRepository repository = services.BuildServiceProvider().GetService<IMongoRepository>();

            IMongoCollection<Establishment> establishmentCollection = repository.GetCollection<Establishment>();
            IMongoCollection<Event> eventCollection = repository.GetCollection<Event>();

            establishmentCollection.Indexes.CreateOne(
                new CreateIndexModel<Establishment>(Builders<Establishment>.IndexKeys.Geo2DSphere(establishmentKey => establishmentKey.Location.Coordinates))
            );

            eventCollection.Indexes.CreateOne(
                new CreateIndexModel<Event>(Builders<Event>.IndexKeys.Geo2DSphere(eventKey => eventKey.Location.Coordinates))
            );
        }
    }
}
