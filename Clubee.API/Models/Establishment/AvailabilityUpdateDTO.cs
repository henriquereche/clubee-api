using Clubee.API.Contracts.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Clubee.API.Models.Establishment
{
    public class AvailabilityUpdateDTO
    {
        [Required]
        public DayOfWeekEnum DayOfWeek { get; set; }

        [Required]
        public TimeSpan OpenTime { get; set; }

        [Required]
        public TimeSpan CloseTime { get; set; }
    }
}
