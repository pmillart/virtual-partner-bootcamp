using ECommerce.Orchestration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace ECommerce.PublicRest
{
    public static class OrderApi
    {
        [FunctionName("OrderCheckoutPost")]
        public static async Task<IActionResult> Checkout(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "user/{userId}/order/checkout")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient client,
            string userId,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var instanceId = await client.StartNewAsync<string>(nameof(OrderOrchestrator), userId);
            return await client.WaitForCompletionOrCreateCheckStatusResponseAsync(req, instanceId);
        }

        [FunctionName("OrderGet")]
        public static async Task<IActionResult> OrderStatus(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/{userId}/order/{orderId}")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
