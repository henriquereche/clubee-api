using Clubee.API.Contracts.Entities;
using Clubee.API.Contracts.Enums;
using Clubee.API.Contracts.Exceptions;
using Clubee.API.Contracts.Extensions;
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clubee.API.Entities
{
    public class Establishment : IMongoEntity
    {
        protected Establishment()
        {
            this.Availabilities = new List<Availability>();
            this.EstablishmentTypes = new List<EstablishmentTypeEnum>();
        }

        public Establishment(
            string name, 
            string image, 
            string imageThumbnail, 
            string description, 
            GeoJson2DGeographicCoordinates location,
            IEnumerable<EstablishmentTypeEnum> establishmentTypes,
            IEnumerable<Availability> availabilities
            ) : this()
        {
            this.Name = name;
            this.Image = image;
            this.ImageThumbnail = imageThumbnail;
            this.Description = description;
            this.Location = location 
                ?? throw new ApplicationValidationException("Establishment location should be specifyed.");

            if (availabilities.IsNullOrEmpty())
                throw new ApplicationValidationException($"Missing establishment availability.");

            if (establishmentTypes.IsNullOrEmpty())
                throw new ApplicationValidationException($"Establishment should have at least one type.");

            foreach (EstablishmentTypeEnum establishmentType in establishmentTypes)
                this.AddEstablishmentType(establishmentType);

            foreach (Availability availability in availabilities)
                this.AddAvailability(availability);
        }

        public ObjectId Id { get; protected set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string ImageThumbnail { get; set; }
        public string Description { get; set; }
        public GeoJson2DGeographicCoordinates Location { get; set; }
        public ICollection<EstablishmentTypeEnum> EstablishmentTypes { get; protected set; }
        public ICollection<Availability> Availabilities { get; protected set; }

        /// <summary>
        /// Add a new availability to establishment.
        /// </summary>
        /// <param name="availability"></param>
        public void AddAvailability(Availability availability)
        {
            if (this.Availabilities.Any(x =>
                (x.CloseTime <= availability.OpenTime && x.DayOfWeek == availability.DayOfWeek)
                || ((x.OpenTime + x.Duration) >= TimeSpan.FromHours(24) && ((int)x.DayOfWeek % 7) + 1 == (int)availability.DayOfWeek % 7)
            ))
                throw new ApplicationValidationException(
                    $"Availability {availability.DayOfWeek} {availability.OpenTime} matches existing availability.");

            this.Availabilities.Add(availability);
        }

        /// <summary>
        /// Add new establishment type to establishment.
        /// </summary>
        /// <param name="establishmentType"></param>
        public void AddEstablishmentType(EstablishmentTypeEnum establishmentType)
        {
            if (!this.EstablishmentTypes.Any(x => x == establishmentType))
                this.EstablishmentTypes.Add(establishmentType);
        }
    }
}
