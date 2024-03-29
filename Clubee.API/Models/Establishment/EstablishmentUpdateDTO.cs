﻿using Clubee.API.Contracts.Enums;
using Clubee.API.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clubee.API.Models.Establishment
{
    public class EstablishmentUpdateDTO
    {
        [Required, MinLength(2)]
        public string Name { get; set; }

        public string Image { get; set; }

        [Required, MinLength(10)]
        public string Description { get; set; }

        public LocationModel Location { get; set; }

        [Required, MinLength(1)]
        public IEnumerable<EstablishmentTypeEnum> EstablishmentTypes { get; set; }

        public IEnumerable<AvailabilityUpdateDTO> Availabilities { get; set; }
    }
}
