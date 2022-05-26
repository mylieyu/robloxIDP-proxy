using System;
using System.Configuration;
using System.Net.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace roblox
{
    public static class roblox
    {
        [FunctionName("roblox")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // read the request body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            // build the new request body
            HttpContent contentPost = new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json");

            // build new http client
            HttpClient client = new();

            // set google sheets api url
            string uri = Environment.GetEnvironmentVariable("GoogleSheetApiUrl");

            // post data to google sheets api
            var response = await client.PostAsync(uri, contentPost);

            // read the response from google sheets spi
            dynamic responseBody = await response.Content.ReadAsAsync<dynamic>();

            // return the response to roblox
            return new OkObjectResult(responseBody);
        }
    }
}
