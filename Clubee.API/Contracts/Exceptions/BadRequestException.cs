using System.Net;

namespace Clubee.API.Contracts.Exceptions
{
    /// <summary>
    /// Exeption to represent 400 bad request response.
    /// </summary>
    public class BadRequestException : ApplicationResponseException
    {
        public BadRequestException(string message) : base(message, HttpStatusCode.BadRequest) { }
    }
}
