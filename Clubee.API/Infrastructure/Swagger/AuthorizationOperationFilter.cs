using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Clubee.API.Infrastructure.Swagger
{
    /// <summary>
    /// Operation filter to set Authorization header on authorized methods. 
    /// </summary>
    public class AuthorizationOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            object[] attribute = context.MethodInfo
                .GetCustomAttributes(typeof(AuthorizeAttribute), true);

            if (attribute != null && attribute.Any())
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Authorization",
                    In = "header",
                    Type = "string",
                    Required = true
                });
        }
    }
}
