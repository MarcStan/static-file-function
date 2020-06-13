using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;

namespace StaticFileFunction
{
    public static class ApiFunction
    {
        [FunctionName("api")]
        public static string Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "time")] HttpRequest req)
        {
            return DateTimeOffset.UtcNow.TimeOfDay.ToString();
        }
    }
}
