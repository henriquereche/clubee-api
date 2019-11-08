using Clubee.API.Contracts.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Clubee.API.Contracts.Infrastructure.Data
{
    public interface IMongoRepository
    {
        /// <summary>
        /// Returns IMongoCollection of required entity type.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IMongoCollection<TEntity> GetCollection<TEntity>()
            where TEntity : class, IMongoEntity;

        /// <summary>
        /// Inserts a new document.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Insert<TEntity>(TEntity entity)
            where TEntity : class, IMongoEntity;

        /// <summary>
        /// Updates existing document.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Update<TEntity>(TEntity entity)
            where TEntity : class, IMongoEntity;

        /// <summary>
        /// Find existing entity by its identifier.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity FindById<TEntity>(string id)
            where TEntity : class, IMongoEntity;

        /// <summary>
        /// Removes existing entity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Remove<TEntity>(TEntity entity)
            where TEntity : class, IMongoEntity;

        /// <summary>
        /// Removes existing entity by its identifier.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Remove<TEntity>(ObjectId id)
           where TEntity : class, IMongoEntity;

        /// <summary>
        /// Query and filter.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : class, IMongoEntity;

        /// <summary>
        /// Query and filter to search existing documents matching filter.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        bool Exists<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : class, IMongoEntity;

        /// <summary>
        /// Query and filter.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Find<TEntity>(string filter)
            where TEntity : class, IMongoEntity;

        /// <summary>
        /// Query paginated and filter.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindPaged<TEntity>(Expression<Func<TEntity, bool>> filter, int pageSize, int page)
            where TEntity : class, IMongoEntity;

        /// <summary>
        /// Query paginated and filter then projects to specified format.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="filter"></param>
        /// <param name="projection"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        IEnumerable<TResult> FindPagedAndProject<TEntity, TResult>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> projection,
            int pageSize,
            int page
        ) where TEntity : class, IMongoEntity;
    }
}
