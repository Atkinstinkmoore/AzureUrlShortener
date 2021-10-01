using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Azure.Documents.Client;
using System.Linq;
using Microsoft.Azure.Documents;

namespace api
{
    public static class GetUrlStats
    {
        [FunctionName("GetUrlStats")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.User, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "UrlShortener",
                collectionName: "UrlTables",
                ConnectionStringSetting = "CosmosDBConnection"
            )] IEnumerable<UrlTable> tables,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var result = tables.Where(t => t.CreatedBy == req.Query["userName"]).ToList();

            if (result == null || result.Count == 0)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(result);
        }
    }
}
