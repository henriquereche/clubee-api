using Clubee.API.Contracts.Enums;
using Clubee.API.Models.Filters.Base;

namespace Clubee.API.Models.Filters
{
    public class EstablishmentFilter : GeospatialBaseFilter
    {
        public string Query { get; set; }
        public EstablishmentTypeEnum? EstablishmentType { get; set; }
        public OrderTypeEnum? OrderType { get; set; }

        /// <summary>
        /// Filter string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"query={this.Query},longitude={this.Longitude}," +
                $"latitude={this.Latitude},meters={this.Meters}," +
                $"establishmentType={this.EstablishmentType}";
        }
    }
}
