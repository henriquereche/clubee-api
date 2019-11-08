using Clubee.API.Contracts.Entities;
using Clubee.API.Contracts.Infrastructure.Data;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Clubee.API.Infrastructure.Data
{
    public class MongoRepository : IMongoRepository
    {
        private readonly IMongoConnection MongoConnection;

        public MongoRepository(IMongoConnection mongoConnection)
        {
            this.MongoConnection = mongoConnection;
        }

        /// <summary>
        /// Returns IMongoCollection of required entity type.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IMongoCollection<TEntity> GetCollection<TEntity>()
            where TEntity : class, IMongoEntity => this.MongoConnection.Database.GetCollection<TEntity>(typeof(TEntity).Name);

        /// <summary>
        /// Inserts a new document.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity Insert<TEntity>(TEntity entity)
            where TEntity : class, IMongoEntity
        {
            this.GetCollection<TEntity>().InsertOne(entity);
            return entity;
        }

        /// <summary>
        /// Updates existing document.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity Update<TEntity>(TEntity entity)
            where TEntity : class, IMongoEntity
        {
            this.GetCollection<TEntity>().ReplaceOne(filter => filter.Id == entity.Id, entity);
            return entity;
        }

        /// <summary>
        /// Find existing entity by its identifier.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity FindById<TEntity>(ObjectId id)
            where TEntity : class, IMongoEntity
        {
            return this.GetCollection<TEntity>()
                .Find(filter => filter.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Removes existing entity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity Remove<TEntity>(TEntity entity)
            where TEntity : class, IMongoEntity
        {
            this.GetCollection<TEntity>().DeleteOne(filter => filter.Id == entity.Id);
            return entity;
        }

        /// <summary>
        /// Removes existing entity by its identifier.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity Remove<TEntity>(ObjectId id)
            where TEntity : class, IMongoEntity
        {
            return this.GetCollection<TEntity>()
                .FindOneAndDelete(filter => filter.Id == id);
        }

        /// <summary>
        /// Query and filte.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : class, IMongoEntity
        {
            return this.GetCollection<TEntity>()
                .Find(filter).ToList();
        }

        /// <summary>
        /// Query and filter to search existing documents matching filter.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool Exists<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : class, IMongoEntity
        {
            return this.GetCollection<TEntity>()
                .Find(filter).Any();
        }

        /// <summary>
        /// Query and filter.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Find<TEntity>(string filter)
            where TEntity : class, IMongoEntity
        {
            return this.GetCollection<TEntity>()
                .Find(filter).ToList();
        }

        /// <summary>
        /// Query paginated and filter.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> FindPaged<TEntity>(Expression<Func<TEntity, bool>> filter, int pageSize, int page)
            where TEntity : class, IMongoEntity
        {
            return this.GetCollection<TEntity>().Find(filter)
                .Skip((page - 1) * pageSize).Limit(pageSize).ToList();
        }

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
        public virtual IEnumerable<TResult> FindPagedAndProject<TEntity, TResult>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> projection,
            int pageSize,
            int page
        ) where TEntity : class, IMongoEntity
        {
            return this.GetCollection<TEntity>()
                .Find(filter)
                .Project(projection)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToList();
        }
    }
}
