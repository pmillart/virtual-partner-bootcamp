using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ECommerce.Domains.Inventory;
using System.Threading;
using System.Collections.Generic;

namespace ECommerce.APIs
{
    public static class Store
    {
        [FunctionName("Store")]
        public static async Task<IEnumerable<InventoryItemView>> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [DurableClient] IDurableEntityClient client,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // string name = req.Query["name"];

            // string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            // dynamic data = JsonConvert.DeserializeObject(requestBody);
            // name = name ?? data?.name;

            // string responseMessage = string.IsNullOrEmpty(name)
            //     ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //     : $"Hello, {name}. This HTTP triggered function executed successfully.";

            var entityId = new EntityId("Store", "customerkey1");
            var cancellationToken = new CancellationToken();
            var state = await client.ReadEntityStateAsync<IInventoryService>(entityId);

            return await state.EntityState.GetCustomerInventoryAsync(cancellationToken);
        }
    }
}
