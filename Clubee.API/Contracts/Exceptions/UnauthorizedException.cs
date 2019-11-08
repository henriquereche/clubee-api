using System.Net;

namespace Clubee.API.Contracts.Exceptions
{
    /// <summary>
    /// Exeption to represent 401 unauthorized response.
    /// </summary>
    public class UnauthorizedException : ApplicationResponseException
    {
        public UnauthorizedException(string message = default) : base(message, HttpStatusCode.Unauthorized) { }
    }
}
