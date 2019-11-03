using System.Net;

namespace Clubee.API.Contracts.Exceptions
{
    /// <summary>
    /// Exeption to represent 500 internal server error response.
    /// </summary>
    public class InternalServerErrorException : ApplicationResponseException
    {
        public InternalServerErrorException(string message) : base(message, HttpStatusCode.InternalServerError) { }
    }
}
