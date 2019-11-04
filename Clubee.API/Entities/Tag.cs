using Clubee.API.Contracts.Entities;
using MongoDB.Bson;

namespace Clubee.API.Entities
{
    public class Tag : IMongoEntity
    {
        public Tag(string name)
        {
            this.Name = name;
        }

        public ObjectId Id { get; protected set; }
        public string Name { get; protected set; }
    }
}
