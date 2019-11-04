using Clubee.API.Contracts.Enums;
using System;

namespace Clubee.API.Entities
{
    public class Availability
    {
        public Availability(
            DayOfWeekEnum dayOfWeek, 
            TimeSpan openTime, 
            TimeSpan closeTime
            )
        {
            this.DayOfWeek = dayOfWeek;
            this.OpenTime = openTime;
            this.CloseTime = closeTime;
        }

        public DayOfWeekEnum DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }

        /// <summary>
        /// Represents available duration.
        /// </summary>
        public TimeSpan Duration => this.CloseTime < this.OpenTime
            ? (TimeSpan.FromHours(24) - this.OpenTime) + this.CloseTime
            : this.CloseTime - this.OpenTime;
    }
}
