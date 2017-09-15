namespace Cql.Core.Web
{
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    public static class ExtensionsForHttpClient
    {
        /// <summary>
        /// Writes debug output of the Request to a string similar to:
        /// <para>POST /api/upload HTTP/1.1</para>
        /// <para>Host: localhost</para>
        /// <para>Content-Type: multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW</para>
        /// <para></para>
        /// <para>------WebKitFormBoundary7MA4YWxkTrZu0gW</para>
        /// <para>Content-Disposition: form-data; name="files[]"; filename=""</para>
        /// <para>Content-Type: </para>
        /// <para></para>
        /// <para>------WebKitFormBoundary7MA4YWxkTrZu0gW--</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<string> ToDebugHttpStringAsync(this HttpRequestMessage request)
        {
            using (var sw = new StringWriter())
            {
                await sw.WriteAsync("\n");
                await sw.WriteLineAsync($"{request.Method.Method} {request.RequestUri.PathAndQuery} HTTP/1.1");
                await sw.WriteLineAsync($"Host: {request.RequestUri.Host}");

                await PrintAllHeaders(request, sw);

                await sw.WriteAsync("\n");
                await sw.WriteLineAsync(await request.Content.ReadAsStringAsync());

                return sw.ToString();
            }
        }

        private static async Task PrintAllHeaders(HttpRequestMessage request, TextWriter sw)
        {
            foreach (var header in request.Headers)
            {
                await sw.WriteLineAsync($"{header.Key}: {string.Join(",", header.Value)}");
            }

            foreach (var header in request.Content.Headers)
            {
                await sw.WriteLineAsync($"{header.Key}: {string.Join(",", header.Value)}");
            }
        }
    }
}
