using Clubee.API.Contracts.Enums;
using Clubee.API.Models.Filters.Base;
using System;

namespace Clubee.API.Models.Filters
{
    public class EventFilter : GeospatialBaseFilter
    {
        public string EstablishmentId { get; set; }
        public string Query { get; set; }
        public GenreEnum? Genre { get; set; }
        public OrderTypeEnum? OrderType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Determines if filter should apply date range filter.
        /// </summary>
        internal bool DateFilter => this.StartDate.HasValue 
            && this.EndDate.HasValue 
            && (this.EndDate.Value > this.StartDate.Value);

        /// <summary>
        /// Filter string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString() 
        { 
            return $"establishmentId={EstablishmentId},query={this.Query}," +
                $"longitude={this.Longitude},latitude={this.Latitude}," +
                $"meters={this.Meters},genre={this.Genre},startDate={this.StartDate}," +
                $"endDate={this.EndDate}";
        }
    }
}
