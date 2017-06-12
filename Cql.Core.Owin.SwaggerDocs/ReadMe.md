Easily add Identity endpoint documentation to the Swagger documentation endpoint.


    public static class SwaggerConfig
    {
        public static void Register(HttpConfiguration httpConfiguration)
        {
            httpConfiguration
                .EnableSwagger(
                    c =>
                    {
                        // Standard Swagger stuff.
                        c.SingleApiVersion("v1", "Sample Web API");

                        // Adds some CQL secret sauce.
                        c.ApplyCqlDefaults();

                        // Adds the Swagger UI endpoints for the /api/token endpoint(s).
                        c.AddStandardAuthTokenDocumentation();

                        // Optionally include XML documentation files to include comments in generated Swagger UI.
                        c.IncludeXmlFiles("Sample.Namespace.Core", "Sample.Namespace.Web");
                    })
                .EnableSwaggerUi(
                    c =>
                    {
                        // Standard Swagger stuff, the sets the default expansion to list each endpoint as a summary.
                        c.DocExpansion(DocExpansion.List);
                    });
        }
    }
