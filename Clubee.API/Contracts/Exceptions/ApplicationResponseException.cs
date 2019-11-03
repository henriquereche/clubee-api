using System;
using System.Net;

namespace Clubee.API.Contracts.Exceptions
{
    /// <summary>
    /// Base exception to represent http status code response.
    /// </summary>
    public abstract class ApplicationResponseException : Exception
    {
        public ApplicationResponseException(
            string message,
            HttpStatusCode statusCode = HttpStatusCode.BadRequest
            ) : base(message)
        {
            this.StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; private set; }
    }
}
