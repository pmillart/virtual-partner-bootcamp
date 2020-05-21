using ECommerce.Domain.ShoppingCart;
using ECommerce.Entities;
using ECommerce.Orchestration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ECommerce.PublicRest
{
    public static class ShoppingCartApi
    {
        [FunctionName("ShoppingCartGet")]
        public static async Task<IActionResult> ShoppingCartGet(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/{userId}/shoppingCart")] HttpRequest req,
            [DurableClient] IDurableClient client,
            string userId,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var instanceId = await client.StartNewAsync<string>(nameof(ShoppingCartOrchestrator), userId);
            return await client.WaitForCompletionOrCreateCheckStatusResponseAsync(req, instanceId);
        }

        [FunctionName("ShoppingCartPost")]
        public static async Task<IActionResult> ShoppingCartPost(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "user/{userId}/shoppingCart/{itemId}")] HttpRequest req,
            [DurableClient] IDurableClient client,
            string userId,
            string itemId,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var target = new EntityId(nameof(ShoppingCartEntity), userId);
            await client.SignalEntityAsync<IShoppingCart>(target, async x => await x.AddItemAsync(itemId));

            return new AcceptedResult();
        }

        [FunctionName("ShoppingCartDelete")]
        public static async Task<IActionResult> ShoppingCartDelete(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "user/{userId}/shoppingCart/{itemId}")] HttpRequest req,
            [DurableClient] IDurableClient client,
            string userId,
            string itemId,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var target = new EntityId(nameof(ShoppingCartEntity), userId);
            await client.SignalEntityAsync<IShoppingCart>(target, async x => await x.RemoveItemAsync(itemId));

            return new AcceptedResult();
        }
    }
}
