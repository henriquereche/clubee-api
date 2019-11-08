using System.Net;

namespace Clubee.API.Contracts.Exceptions
{

    /// <summary>
    /// Exeption to represent 404 not found response.
    /// </summary>
    public class NotFoundException : ApplicationResponseException
    {
        public NotFoundException(string message = default) : base(message, HttpStatusCode.NotFound) { }
    }
}
