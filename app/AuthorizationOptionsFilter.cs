namespace sunstealer.mvc
{
    public class AuthorizationOptionsFilter : Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
    {
        public void Apply(Microsoft.OpenApi.Models.OpenApiOperation operation, Swashbuckle.AspNetCore.SwaggerGen.OperationFilterContext context)
        {
            if(context.MethodInfo.DeclaringType != null) {
                var hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>().Any()
                  || context.MethodInfo.GetCustomAttributes(true).OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>().Any();

                if (hasAuthorize)
                {
                    operation.Responses.TryAdd("401", new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Unauthorized" });
                    operation.Responses.TryAdd("403", new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Forbidden" });

                    operation.Security = new List<Microsoft.OpenApi.Models.OpenApiSecurityRequirement>
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                    {
                        [
                            new Microsoft.OpenApi.Models.OpenApiSecurityScheme {Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "oauth2"}
                            }
                        ] = new[] {"api1"}
                    }
                };
                }
            }
        }
    }
}
