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
using System.Net;

namespace api.SaveUrl
{
    public static class SaveUrl
    {
        [FunctionName("SaveUrl")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
             [CosmosDB(databaseName: "UrlShortener", collectionName: "UrlTables", 
                       ConnectionStringSetting = "CosmosDBConnection")]DocumentClient client,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            //läs och deserialisera request body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
           
            //om requestbody är tom, fucka the fuck out
            if (requestBody == null)
            {
                return new NotFoundResult();
            }

            dynamic data = JsonConvert.DeserializeObject(requestBody);
            
            string name = data.CreatedBy;
            string url = data.LongUrl;

            // kolla att LongUrl och CreatedBy har värden som de ska
            if(!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(url))
                {
                    //skapa upp doc client
                    var option = new FeedOptions { EnableCrossPartitionQuery = true };
                    Uri urlCollectionUri = UriFactory.CreateDocumentCollectionUri("UrlShortener", "UrlTables");

                    //gör anrop till DB och fråga om antal inlägg. lägg i int.ToString för id
                    var result = client.CreateDocumentQuery<UrlTable>(urlCollectionUri, option).AsEnumerable().Count();

                    //skapa upp TR
                    UrlTable tableRow = new UrlTable{
                        Id = (result +1).ToString(),
                        LongUrl = data.LongUrl,
                        CreatedBy = data.CreatedBy,
                        CreatedAt = DateTimeOffset.Now,
                        TimesClicked = 0
                    };

                    //skriv tableRow till DB

                    var response = (await client.CreateDocumentAsync(urlCollectionUri, tableRow)).StatusCode;

                
                    if (response == HttpStatusCode.Created)
                        return new OkResult();
                }
           
                return new NotFoundResult();

        }
    }
}
