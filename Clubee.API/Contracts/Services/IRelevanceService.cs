using Clubee.API.Contracts.Entities;
using Clubee.API.Models.Filters;
using MongoDB.Bson;

namespace Clubee.API.Contracts.Services
{
    public interface IRelevanceService
    {
        /// <summary>
        /// Register relevance count using provided filter.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TFilter"></typeparam>
        /// <param name="filter"></param>
        void Register<TEntity, TFilter>(TFilter filter)
            where TEntity : class, IMongoEntity
            where TFilter : BaseFilter;

        /// <summary>
        /// Register relevance count using provided identifier.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        void Register<TEntity>(ObjectId id)
            where TEntity : class, IMongoEntity;
    }
}
