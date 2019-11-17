using Clubee.API.Contracts.Entities;
using Clubee.API.Contracts.Extensions;
using Clubee.API.Contracts.Services;
using Clubee.API.Entities;
using Clubee.API.Models.Filters;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Clubee.API.Services
{
    public class RelevanceService : IRelevanceService
    {
        public const int FilterRelevanceCount = 1;
        public const int IdentifierRelevanceCount = 10;

        private readonly IHttpContextAccessor ContextAccessor;
        private readonly RelevanceEmitter RelevanceEmitter;

        public RelevanceService(IHttpContextAccessor contextAccessor)
        {
            this.ContextAccessor = contextAccessor;
            this.RelevanceEmitter = RelevanceEmitter.Instance;
        }

        /// <summary>
        /// Register relevance count using provided filter.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TFilter"></typeparam>
        /// <param name="filter"></param>
        public void Register<TEntity, TFilter>(TFilter filter)
            where TEntity : class, IMongoEntity
            where TFilter : BaseFilter
        {
            string filterKey = filter.ToString();
            string hostAddress = this.ContextAccessor.HttpContext.Request.Host.Value;

            this.RelevanceEmitter.Add(
                $"{hostAddress}-{filterKey}",
                new RelevanceCount(RelevanceService.FilterRelevanceCount, typeof(TEntity).Name, filter.ToDictionary())
            );
        }

        /// <summary>
        /// Register relevance count using provided identifier.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        public void Register<TEntity>(ObjectId id)
            where TEntity : class, IMongoEntity
        {
            string hostAddress = this.ContextAccessor.HttpContext.Request.Host.Value;

            this.RelevanceEmitter.Add(
                $"{hostAddress}-{id.ToString()}",
                new RelevanceCount(RelevanceService.IdentifierRelevanceCount, typeof(TEntity).Name, new Dictionary<string, string> { { "Id", id.ToString() } })
            );
        }
    }
}
