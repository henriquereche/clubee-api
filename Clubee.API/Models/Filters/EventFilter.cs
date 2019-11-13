using Clubee.API.Contracts.Enums;

namespace Clubee.API.Models.Filters
{
    public class EventFilter : BaseFilter
    {
        public string EstablishmentId { get; set; }
        public string Query { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public double? Meters { get; set; }
        public GenreEnum? Genre { get; set; }

        internal bool GeospatialQuery => 
            this.Longitude.HasValue && this.Latitude.HasValue;
    }
}
