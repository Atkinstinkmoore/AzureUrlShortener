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
using Microsoft.Azure.Documents.Linq;

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
            )] DocumentClient client,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["userName"];

            if (string.IsNullOrWhiteSpace(name))
            {
                return new NotFoundResult();
            }

            var option = new FeedOptions { EnableCrossPartitionQuery = true };
            Uri urlCollectionUri = UriFactory.CreateDocumentCollectionUri("UrlShortener", "UrlTables");

            var result = client.CreateDocumentQuery<UrlTable>(urlCollectionUri, option).AsEnumerable().Where(t => t.CreatedBy == name).ToList();


            if (result == null || result.ToList().Count == 0)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(result);
        }
    }
}
