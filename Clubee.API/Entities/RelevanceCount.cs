using Clubee.API.Contracts.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Clubee.API.Entities
{
    public class RelevanceCount : IMongoEntity
    {
        protected RelevanceCount()
        {
            this.Integrated = false;
            this.Expiration = DateTime.Now.AddDays(30);
        }

        public RelevanceCount(
            int count,
            string collection,
            IDictionary<string, string> properties
            ) : this()
        {
            this.Count = count;
            this.Collection = collection;
            this.Properties = properties;
        }

        public ObjectId Id { get; protected set; }
        public int Count { get; protected set; }
        public DateTime Expiration { get; protected set; }
        public string Collection { get; protected set; }
        public IDictionary<string, string> Properties { get; protected set; }
        public bool Integrated { get; protected set; }
    }
}
