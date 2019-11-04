using Clubee.API.Contracts.Entities;
using Clubee.API.Contracts.Exceptions;
using Clubee.API.Contracts.Extensions;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clubee.API.Entities
{
    public class Event : IMongoEntity
    {
        protected Event()
        {
            this.Tags = new List<Tag>();
        }

        public Event(
            ObjectId businessId,
            DateTime startDate,
            DateTime endDate,
            string name,
            string description,
            string image,
            IEnumerable<Tag> tags)
        {
            this.BusinessId = businessId;
            this.Name = name;
            this.Description = description;
            this.Image = image;

            this.SetDate(
                startDate,
                endDate
            );

            if (!tags.IsNullOrEmpty()) foreach (Tag tag in tags)
                this.AddTag(tag);
        }

        public ObjectId Id { get; protected set; }
        public ObjectId BusinessId { get; protected set; }
        public DateTime StartDate { get; protected set; }
        public DateTime EndDate { get; protected set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public ICollection<Tag> Tags { get; protected set; }

        /// <summary>
        /// Represent total event duration.
        /// </summary>
        public TimeSpan Duration => this.EndDate - this.StartDate;

        /// <summary>
        /// Add a new tag to event.
        /// </summary>
        /// <param name="tag"></param>
        public void AddTag(Tag tag)
        {
            if (!this.Tags.Any(x => x.Name == tag.Name))
                this.Tags.Add(tag);
        }

        /// <summary>
        /// Set event begin and end date.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public void SetDate(DateTime startDate, DateTime endDate)
        {
            if (endDate <= startDate)
                throw new ApplicationValidationException(
                    $"End date {endDate} must be greater than start date {startDate}.");

            if (startDate <= DateTime.Now)
                throw new ApplicationValidationException(
                    $"Not allowed to register past events.");

            this.StartDate = startDate;
            this.EndDate = endDate;
        }
    }
}
