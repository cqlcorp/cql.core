namespace Cql.Core.Owin.SwaggerDocs
{
    using System;
    using System.IO;
    using System.Linq;

    using Swashbuckle.Application;

    public static class SwaggerDocConfigExtensions
    {
        /// <summary>
        /// Add a sub-classed version of the <see cref="AuthTokenOperation" /> to customize the AuthToken endpoint documentation.
        /// </summary>
        public static void AddAuthTokenDocumentation<TDocumentFilter>(this SwaggerDocsConfig c)
            where TDocumentFilter : AuthTokenOperation, new()
        {
            c.DocumentFilter<TDocumentFilter>();
        }

        /// <summary>
        /// Adds the standard AuthToken endpoint documentation.
        /// </summary>
        /// <param name="c"></param>
        public static void AddStandardAuthTokenDocumentation(this SwaggerDocsConfig c)
        {
            c.DocumentFilter<AuthTokenOperation>();
        }

        public static void ApplyCqlDefaults(this SwaggerDocsConfig c)
        {
            c.OperationFilter<AddAuthorizationHeaderParameterOperationFilter>();
            c.OperationFilter<SupportFlaggedEnums>();
            c.OperationFilter<HideCancellationToken>();

            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            c.DescribeAllEnumsAsStrings();
        }

        public static void IncludeXmlFiles(this SwaggerDocsConfig c, params string[] assemblyNames)
        {
            var binFolder = $@"{AppDomain.CurrentDomain.BaseDirectory}\bin\";

            foreach (var assemblyName in assemblyNames)
            {
                var filePath = Path.Combine(binFolder, $"{assemblyName}.xml");

                if (File.Exists(filePath))
                {
                    c.IncludeXmlComments(filePath);
                }
            }
        }
    }
}
