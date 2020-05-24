using ECommerce.Domain.Inventory;
using ECommerce.Domain.Order;
using ECommerce.Domain.ShoppingCart;
using ECommerce.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Orchestration
{
    public static class OrderOrchestrator
    {
        [FunctionName("OrderOrchestrator")]
        public static async Task<bool> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var userId = context.GetInput<string>();
            var shoppingCartEntity = new EntityId(nameof(ShoppingCartEntity), userId);
            var inventoryEntity = new EntityId(nameof(InventoryEntity), "onestore");
            var orderEntity = new EntityId(nameof(OrderEntity), userId);

            // Create a critical section to avoid race conditions.
            using (await context.LockAsync(inventoryEntity, orderEntity, shoppingCartEntity))
            {
                IShoppingCart shoppingCartProxy = 
                    context.CreateEntityProxy<IShoppingCart>(shoppingCartEntity);
                IInventory inventoryProxy =
                    context.CreateEntityProxy<IInventory>(inventoryEntity);
                IOrder orderProxy =
                    context.CreateEntityProxy<IOrder>(orderEntity);
                
                var shoppingCartItems = await shoppingCartProxy.GetItemsAsync();
                var orderItem = new OrderItem()
                {
                    Timestamp = DateTime.UtcNow,
                    UserId = userId,
                    Details = shoppingCartItems
                };

                var canSell = true;
                foreach (var inventoryItem in orderItem.Details)
                {
                    if (await inventoryProxy.IsItemInInventory(inventoryItem))
                    {
                        await inventoryProxy.RemoveStockAsync(inventoryItem);
                        await shoppingCartProxy.RemoveItemAsync(inventoryItem);
                    }
                    else
                    {
                        canSell = false;
                        break;
                    }
                }

                if (canSell)
                {
                    await orderProxy.AddAsync(orderItem);
                    // order placed successfully
                    return true;
                }
                // the order failed due to insufficient stock
                return false;
            }
        }
    }
}