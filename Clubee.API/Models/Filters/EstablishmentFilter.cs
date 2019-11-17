using Clubee.API.Contracts.Enums;

namespace Clubee.API.Models.Filters
{
    public class EstablishmentFilter : BaseFilter
    {
        public string Query { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public double? Meters { get; set; }
        public EstablishmentTypeEnum? EstablishmentType { get; set; }
        public OrderTypeEnum? OrderType { get; set; }

        internal bool GeospatialQuery =>
            this.Longitude.HasValue && this.Latitude.HasValue;

        public override string ToString() 
            => $"query={this.Query},longitude={this.Longitude},latitude={this.Latitude},meters={this.Meters},establishmentType={this.EstablishmentType}";
    }
}
