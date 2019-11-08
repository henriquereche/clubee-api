using Clubee.API.Contracts.Enums;
using Clubee.API.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clubee.API.Models.Establishment
{
    public class EstablishmentUpdateDTO
    {
        [Required, MinLength(2)]
        public string Name { get; set; }

        [Required]
        public string Image { get; set; }

        [Required, MinLength(10)]
        public string Description { get; set; }

        [Required]
        public LocationModel Location { get; set; }

        [Required, MinLength(1)]
        public IEnumerable<EstablishmentTypeEnum> EstablishmentTypes { get; set; }

        [Required, MinLength(1)]
        public IEnumerable<AvailabilityUpdateDTO> Availabilities { get; set; }
    }
}
