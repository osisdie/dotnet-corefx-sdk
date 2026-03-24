using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CoreFX.Hosting.Middlewares
{
    public class RemoveDefaultApiVersionRouteDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var apiDescription in context.ApiDescriptions)
            {
                if (apiDescription.GroupName != "latest")
                {
                    continue;
                }

                var route = "/" + apiDescription.RelativePath.TrimEnd('/');
                swaggerDoc.Paths.Remove(route);
            }
        }
    }
}
