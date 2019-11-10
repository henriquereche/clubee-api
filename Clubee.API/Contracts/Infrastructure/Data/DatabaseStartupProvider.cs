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

            establishmentCollection.Indexes.CreateOne(
                new CreateIndexModel<Establishment>(
                    Builders<Establishment>.IndexKeys.Text(establishmentKey => establishmentKey.Description)
                        .Text(establishmentKey => establishmentKey.Name)
                        .Text(establishmentKey => establishmentKey.Location.Street)
                        .Text(establishmentKey => establishmentKey.Location.Number),
                    new CreateIndexOptions { DefaultLanguage = "portuguese" }
                )
            ); 

            eventCollection.Indexes.CreateOne(
                new CreateIndexModel<Event>(Builders<Event>.IndexKeys.Geo2DSphere(eventKey => eventKey.Location.Coordinates))
            );

            eventCollection.Indexes.CreateOne(
                new CreateIndexModel<Event>(
                    Builders<Event>.IndexKeys.Text(eventKey => eventKey.Name)
                        .Text(eventKey => eventKey.Description)
                        .Text(eventKey => eventKey.Location.Street)
                        .Text(eventKey => eventKey.Location.Number),
                    new CreateIndexOptions { DefaultLanguage = "portuguese" }
                )
            );
        }
    }
}
