using System;

namespace Clubee.API.Models.User
{
    public class UserLoginResultDTO
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
