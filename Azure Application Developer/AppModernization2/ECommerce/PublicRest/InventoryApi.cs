using ECommerce.Domain.Inventory;
using ECommerce.Entities;
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
    public static class InventoryApi
    {
        [FunctionName("InventoryCreateItemPost")]
        public static async Task<IActionResult> CreateInventoryItem(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "store/inventory")] HttpRequest req,
            [DurableClient] IDurableClient client,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var item = JsonConvert.DeserializeObject<InventoryItem>(requestBody);
            var target = new EntityId(nameof(InventoryEntity), "onestore");

            await client.SignalEntityAsync<IInventory>(target, async x => await x.CreateItemAsync(item));

            return new AcceptedResult();
        }

        [FunctionName("InventoryAddItemStockPut")]
        public static async Task<IActionResult> InventoryAddItemStockPut(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "store/inventory/{itemId}")] HttpRequest req,
            [DurableClient] IDurableClient client,
            string itemId,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var target = new EntityId(nameof(InventoryEntity), "onestore");

            await client.SignalEntityAsync<IInventory>(target, x => x.AddStock(itemId));

            return new AcceptedResult();
        }
    }
}
