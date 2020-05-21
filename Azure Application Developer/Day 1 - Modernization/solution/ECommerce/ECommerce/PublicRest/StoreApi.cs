using ECommerce.Domain.Inventory;
using ECommerce.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.PublicRest
{
    public static class StoreApi
    {
        [FunctionName("StoreGet")]
        public static async Task<IActionResult> StoreGet(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "store")] HttpRequest req,
            [DurableClient] IDurableClient client,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var target = new EntityId(nameof(InventoryEntity), "onestore");
            var store = await client.ReadEntityStateAsync<InventoryEntity>(target);

            return new JsonResult(store.EntityState?.Items ?? new List<InventoryItem>());
        }
    }
}
