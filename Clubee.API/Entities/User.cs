using Clubee.API.Contracts.Entities;
using MongoDB.Bson;

namespace Clubee.API.Entities
{
    public class User : IMongoEntity
    {
        public User(
            ObjectId businessId, 
            string email, 
            string password, 
            string salt
            )
        {
            this.BusinessId = businessId;
            this.Email = email;

            this.SetPassword(password, salt);
        }

        public ObjectId Id { get; protected set; }
        public ObjectId BusinessId { get; protected set; }
        public string Email { get; protected set; }
        public string Password { get; protected set; }
        public string Salt { get; protected set; }

        /// <summary>
        /// Set user password and salt.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        public void SetPassword(string password, string salt)
        {
            this.Password = password;
            this.Salt = salt;
        }
    }
}
