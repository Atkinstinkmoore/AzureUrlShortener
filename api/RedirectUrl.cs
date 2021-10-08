using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Documents.Client;
using System.Linq;

namespace api
{
    public static class RedirectUrl
    {
        [FunctionName("HttpRedirect")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "UrlShortener",
                collectionName: "UrlTables",
                ConnectionStringSetting = "CosmosDBConnection"
            )] DocumentClient client,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string shortUrl = req.Query["Id"];

            if (string.IsNullOrWhiteSpace(shortUrl))
            {
                return new NotFoundResult();
            }

            var option = new FeedOptions { EnableCrossPartitionQuery = true };
            Uri urlCollectionUri = UriFactory.CreateDocumentCollectionUri("UrlShortener", "UrlTables");

            var result = client.CreateDocumentQuery<UrlTable>(urlCollectionUri, option).AsEnumerable().SingleOrDefault(u => u.Id == shortUrl);

            if (result == null || result.LongUrl == "")
            {
                return new NotFoundResult();
            }

            return new RedirectResult(result.LongUrl, true);
        }
    }
}