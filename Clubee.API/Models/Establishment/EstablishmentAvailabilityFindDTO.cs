using Clubee.API.Contracts.Enums;
using System;

namespace Clubee.API.Models.Establishment
{
    public class EstablishmentAvailabilityFindDTO
    {
        public DayOfWeekEnum DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
