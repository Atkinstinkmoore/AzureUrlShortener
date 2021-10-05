using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace api.SaveUrl
{
    public static class SaveUrl
    {
        [FunctionName("SaveUrl")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
             [CosmosDB(databaseName: "UrlShortener", collectionName: "UrlTables", 
                       ConnectionStringSetting = "CosmosDBConnection")]IAsyncCollector<dynamic> documentsOut,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            UrlTable tableRow = JsonConvert.DeserializeObject<UrlTable>(requestBody);

                if (!string.IsNullOrEmpty(requestBody))
                    {
                        // Add a JSON document to the output container.
                        await documentsOut.AddAsync(tableRow);
                    }
            //name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(requestBody)
                ? "This HTTP triggered function executed successfully but your request body was empty."
                : $"Hello. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
