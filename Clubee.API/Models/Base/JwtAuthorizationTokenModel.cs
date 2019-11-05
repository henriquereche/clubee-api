using System;

namespace Clubee.API.Models.Base
{
    public class JwtAuthorizationTokenModel
    {
        public readonly string Token;
        public readonly DateTime Expiration;

        public JwtAuthorizationTokenModel(
            string token, 
            DateTime expiration
            )
        {
            this.Token = token;
            this.Expiration = expiration;
        }
    }
}
