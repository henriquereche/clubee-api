using Clubee.API.Contracts.Entities;
using Clubee.API.Contracts.Enums;
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
            this.Relevance = 0;
            this.Genres = new List<GenreEnum>();
        }

        public Event(
            ObjectId establishmentId,
            DateTime startDate,
            DateTime endDate,
            string name,
            string description,
            Image image,
            Image imageThumbnail,
            Location location,
            bool establishmentLocation,
            IEnumerable<GenreEnum> genres
            ) : this()
        {
            this.EstablishmentId = establishmentId;
            this.Name = name;
            this.Description = description;
            this.Image = image;
            this.ImageThumbnail = imageThumbnail;
            this.Location = location;
            this.EstablishmentLocation = establishmentLocation;

            this.SetDate(
                startDate,
                endDate
            );

            if (!genres.IsNullOrEmpty()) foreach (GenreEnum genre in genres)
                this.AddGenre(genre);
        }

        public ObjectId Id { get; protected set; }
        public ObjectId EstablishmentId { get; protected set; }
        public DateTime StartDate { get; protected set; }
        public DateTime EndDate { get; protected set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Image Image { get; set; }
        public Image ImageThumbnail { get; set; }
        public Location Location { get; set; }
        public bool EstablishmentLocation { get; set; }
        public ICollection<GenreEnum> Genres { get; protected set; }
        public long Relevance { get; protected set; }

        /// <summary>
        /// Represent total event duration.
        /// </summary>
        public TimeSpan Duration => this.EndDate - this.StartDate;

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

        /// <summary>
        /// Add new genre to event.
        /// </summary>
        /// <param name="genre"></param>
        public void AddGenre(GenreEnum genre)
        {
            if (!this.Genres.Any(x => x == genre))
                this.Genres.Add(genre);
        }
    }
}
