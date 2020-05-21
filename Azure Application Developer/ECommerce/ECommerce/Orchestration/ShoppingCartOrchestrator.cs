using ECommerce.Domain.Inventory;
using ECommerce.Domain.ShoppingCart;
using ECommerce.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Orchestration
{
    public static class ShoppingCartOrchestrator
    {
        [FunctionName("ShoppingCartOrchestrator")]
        public static async Task<IEnumerable<InventoryItem>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var userId = context.GetInput<string>();
            var target = new EntityId(nameof(ShoppingCartEntity), userId);

            var shoppingCartProxy = context.CreateEntityProxy<IShoppingCart>(target);
            var shoppingCart = await shoppingCartProxy.GetItemsAsync();

            var tasks = shoppingCart
                .Select(id => context
                    .CreateEntityProxy<IInventory>(new EntityId(nameof(InventoryEntity), "onestore"))
                    .GetItemAsync(id))
                .ToList();

            await Task.WhenAll(tasks);

            var result = tasks
                .Select(task => task.Result)
                .ToList();

            return result;
        }
    }
}