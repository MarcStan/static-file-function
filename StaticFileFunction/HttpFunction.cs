using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using StaticFileFunction.Storage;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StaticFileFunction
{
    public static class HttpFunction
    {
        private static readonly IStorageService _blobStorageHelper;

        static HttpFunction()
        {
            var cfg = new ConfigurationBuilder().AddEnvironmentVariables().Build();

            var account = CloudStorageAccount.Parse(cfg["AzureWebJobsStorage"]);
            _blobStorageHelper = new StorageService(account, cfg);
        }

        [FunctionName("http")]
        public static async Task<HttpResponseMessage> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            var path = req.Query["path"].FirstOrDefault();
            if (string.IsNullOrEmpty(path))
            {
                path = "index.html";
            }
            else if (path.EndsWith("/"))
            {
                path = path.Substring(0, path.Length - 1);
            }
            if (path.Contains("..", StringComparison.OrdinalIgnoreCase))
                return NotFound();

            if (!await _blobStorageHelper.FileExistsAsync(path))
            {
                // default to index for subdirectories as well
                path += "/index.html";
                if (!await _blobStorageHelper.FileExistsAsync(path))
                    return NotFound();
            }

            var data = await _blobStorageHelper.ReadAsync(path);
            return FromStream(data);
        }

        private static HttpResponseMessage NotFound()
            => FromStream(new MemoryStream(Encoding.UTF8.GetBytes("404 Not found")));

        private static HttpResponseMessage FromStream(Stream data)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(data)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
