using Clubee.API.Contracts.Enums;
using Clubee.API.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clubee.API.Models.Event
{
    public class EventUpdateDTO
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required, StringLength(40, MinimumLength = 2)]
        public string Name { get; set; }

        [Required, StringLength(500, MinimumLength = 10)]
        public string Description { get; set; }

        public string Image { get; set; }

        [Required, MinLength(1)]
        public IEnumerable<GenreEnum> Genres { get; set; }

        public LocationModel Location { get; set; }
    }
}
