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
            this.SetTime(openTime, closeTime);
        }

        public DayOfWeekEnum DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; protected set; }
        public TimeSpan CloseTime { get; protected set; }
        public DayOfWeekEnum? CloseDayOfWeek { get; protected set; }

        /// <summary>
        /// Defines availability open and close time.
        /// </summary>
        /// <param name="openTime"></param>
        /// <param name="closeTime"></param>
        public void SetTime(TimeSpan openTime, TimeSpan closeTime)
        {
            this.OpenTime = openTime;
            this.CloseTime = closeTime;

            if (this.CloseTime < this.OpenTime)
            {
                int dayOfWeek = (int)this.DayOfWeek + 1;

                this.CloseDayOfWeek = dayOfWeek > 7
                    ? DayOfWeekEnum.Sunday
                    : (DayOfWeekEnum)dayOfWeek;
            }
        }

        /// <summary>
        /// Represents available duration.
        /// </summary>
        public TimeSpan Duration => this.CloseTime < this.OpenTime
            ? (TimeSpan.FromHours(24) - this.OpenTime) + this.CloseTime
            : this.CloseTime - this.OpenTime;
    }
}
