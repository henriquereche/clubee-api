using Clubee.API.Contracts.Entities;
using Clubee.API.Contracts.Exceptions;
using Clubee.API.Contracts.Extensions;
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clubee.API.Entities
{
    public class Business : IMongoEntity
    {
        protected Business()
        {
            this.Availability = new List<Availability>();
            this.Tags = new List<Tag>();
        }

        public Business(
            string name, 
            string image, 
            string description, 
            GeoJson2DGeographicCoordinates location, 
            IEnumerable<Availability> availability,
            IEnumerable<Tag> tags
            ) : this()
        {
            this.Name = name;
            this.Image = image;
            this.Description = description;
            this.Location = location;

            if (!tags.IsNullOrEmpty()) foreach (Tag tag in tags)
                this.AddTag(tag);

            if (!availability.IsNullOrEmpty()) foreach (Availability item in availability)
                this.AddAvailability(item);
        }

        public ObjectId Id { get; protected set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public GeoJson2DGeographicCoordinates Location { get; set; }
        public ICollection<Availability> Availability { get; set; }
        public ICollection<Tag> Tags { get; protected set; }

        /// <summary>
        /// Add a new availability to business.
        /// </summary>
        /// <param name="availability"></param>
        public void AddAvailability(Availability availability)
        {
            if (this.Availability.Any(x =>
                (x.CloseTime <= availability.OpenTime && x.DayOfWeek == availability.DayOfWeek)
                || ((x.OpenTime + x.Duration) >= TimeSpan.FromHours(24) && ((int)x.DayOfWeek % 7) + 1 == (int)availability.DayOfWeek % 7)
            ))
                throw new ApplicationValidationException(
                    $"Availability {availability.DayOfWeek} {availability.OpenTime} matches existing availability.");

            this.Availability.Add(availability);
        }

        /// <summary>
        /// Add a new tag to business.
        /// </summary>
        /// <param name="tag"></param>
        public void AddTag(Tag tag)
        {
            if (!this.Tags.Any(x => x.Name == tag.Name))
                this.Tags.Add(tag);
        }
    }
}
