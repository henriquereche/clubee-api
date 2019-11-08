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
        public string Address { get; set; }
    }
}
