using Clubee.API.Contracts.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Clubee.API.Infrastructure.Middlewares
{
    public class ApplicationExceptionHandlerMiddleware
    {
        private readonly RequestDelegate Next;
        private readonly ILogger<ApplicationExceptionHandlerMiddleware> Logger;

        public const string DefaultResponseMessage = "Unhandled server error, please contact our support.";
        public const HttpStatusCode DefaultHttpStatusCode = HttpStatusCode.InternalServerError;

        public ApplicationExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<ApplicationExceptionHandlerMiddleware> logger
            )
        {
            this.Next = next;
            this.Logger = logger;
        }

        /// <summary>
        /// Invokes following request and handle application exceptions.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try { await this.Next(context); }
            catch (Exception exception)
            {
                if (exception is ApplicationResponseException)
                    await this.WriteResponse(context.Response, exception);
                else if (exception.InnerException is ApplicationResponseException)
                    await this.WriteResponse(context.Response, exception.InnerException);
                else
                {
                    this.Logger.LogError(exception,
                        $@"Unhandled server error. 
                            path: {context.Request.Path}
                            headers: {JsonConvert.SerializeObject(context.Request.Headers)}
                    ");

                    context.Response.StatusCode = (int)ApplicationExceptionHandlerMiddleware.DefaultHttpStatusCode;
                    await context.Response.WriteAsync(ApplicationExceptionHandlerMiddleware.DefaultResponseMessage);
                }
            }
        }

        /// <summary>
        /// Writes exception message and status code on server response.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public async Task WriteResponse(HttpResponse response, Exception exception)
        {
            response.StatusCode = (int)(exception as ApplicationResponseException).StatusCode;
            await response.WriteAsync(exception.Message);
        }
    }
}
