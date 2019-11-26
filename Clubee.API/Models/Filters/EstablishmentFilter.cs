using Clubee.API.Contracts.Enums;
using Clubee.API.Models.Filters.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clubee.API.Models.Filters
{
    public class EstablishmentFilter : GeospatialBaseFilter
    {
        public string Query { get; set; }
        public EstablishmentTypeEnum? EstablishmentType { get; set; }
        public OrderTypeEnum? OrderType { get; set; }
        public string DaysOfWeek { get; set; }

        /// <summary>
        /// Days of week filter array.
        /// </summary>
        internal IEnumerable<DayOfWeekEnum> DaysOfWeekEnum 
        {
            get 
            {
                try { 
                    return string.IsNullOrEmpty(this.DaysOfWeek) ? null 
                        : DaysOfWeek.Split(',')
                            .Select(value => (DayOfWeekEnum)int.Parse(value.Trim()))
                            .ToList(); 
                }
                catch (Exception) { return null; }
            }
        }

        /// <summary>
        /// Filter string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"query={this.Query},longitude={this.Longitude}," +
                $"latitude={this.Latitude},meters={this.Meters}," +
                $"establishmentType={this.EstablishmentType},daysOfWeek={this.DaysOfWeek}";
        }
    }
}
