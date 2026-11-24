
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API1.Configuration
{
    public class OpenApiAuthOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var actionMetadata =
                context.ApiDescription.ActionDescriptor.EndpointMetadata;

            var markedWithAuthorize = actionMetadata
                .Any(metadataItem => metadataItem is IAuthorizeData);

            var markedWithAllowAnonymous = actionMetadata
                .Any(metadataItem => metadataItem is IAllowAnonymous);

            if (markedWithAuthorize && !markedWithAllowAnonymous)
            {
                var securityScheme =
                    new OpenApiSecuritySchemeReference("Bearer", context.Document);

                operation.Security ??= new List<OpenApiSecurityRequirement>();

                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [securityScheme] = []
                });
            }
        }
    }
}
