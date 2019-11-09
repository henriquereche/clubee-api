using System.ComponentModel.DataAnnotations;

namespace Clubee.API.Models.Base
{
    public class LocationModel
    {
        [Required]
        public double Longitude { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public uint Number { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }
    }
}
