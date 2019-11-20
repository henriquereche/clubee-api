using Clubee.API.Contracts.Enums;
using Clubee.API.Models.Filters.Base;

namespace Clubee.API.Models.Filters
{
    public class EventFilter : GeospatialBaseFilter
    {
        public string EstablishmentId { get; set; }
        public string Query { get; set; }
        public GenreEnum? Genre { get; set; }
        public OrderTypeEnum? OrderType { get; set; }

        /// <summary>
        /// Filter string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString() 
        { 
            return $"establishmentId={EstablishmentId},query={this.Query}," +
                $"longitude={this.Longitude},latitude={this.Latitude}," +
                $"meters={this.Meters},genre={this.Genre}";
        }
    }
}
