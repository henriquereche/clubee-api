namespace Clubee.API.Models.Filters.Base
{
    public class GeospatialBaseFilter : BaseFilter
    {
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public double? Meters { get; set; }

        /// <summary>
        /// Determines if filter should use geoespatial query.
        /// </summary>
        internal bool GeospatialQuery =>
            this.Longitude.HasValue && this.Latitude.HasValue;
    }
}
