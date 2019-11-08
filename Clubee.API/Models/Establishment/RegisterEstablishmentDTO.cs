using Clubee.API.Contracts.Enums;
using Clubee.API.Models.Base;
using Clubee.API.Models.User;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clubee.API.Models.Establishment
{
    public class RegisterEstablishmentDTO
    {
        [Required, MinLength(2)]
        public string Name { get; set; }

        [Required]
        public string Image { get; set; }

        [Required, MinLength(10)]
        public string Description { get; set; }

        public LocationModel Location { get; set; }

        [Required, MinLength(1)]
        public IEnumerable<EstablishmentTypeEnum> EstablishmentTypes { get; set; }

        public IEnumerable<AvailabilityInsertDTO> Availabilities { get; set; }

        [Required]
        public RegisterUserDTO User { get; set; }
    }
}
