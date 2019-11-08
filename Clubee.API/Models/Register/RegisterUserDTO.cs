using System.ComponentModel.DataAnnotations;

namespace Clubee.API.Models.Register
{
    public class RegisterUserDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }
    }
}
