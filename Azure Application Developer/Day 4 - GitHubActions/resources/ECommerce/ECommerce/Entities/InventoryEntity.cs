using ECommerce.Domain.Inventory;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Entities
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class InventoryEntity : IInventory
    {
        [JsonProperty]
        public List<InventoryItem> Items { get; set; } = new List<InventoryItem>();

        public void AddStock(string itemId)
        {
            this.Items.Where(item => item.Id == itemId).FirstOrDefault()?.AddStock(1);
        }

        public Task<bool> RemoveStockAsync(string itemId)
        {
            this.Items.Where(item => item.Id == itemId).FirstOrDefault()?.RemoveStock(1);
            return Task.FromResult(true);
        }

        public Task<bool> IsItemInInventory(string itemId)
        {
            return Task.FromResult((this.Items.Where(item => item.Id == itemId).FirstOrDefault()?.AvailableStock ?? 0) > 0);
        }

        public Task<bool> CreateItemAsync(InventoryItem item)
        {
            this.Items.Add(item);
            return Task.FromResult(true);
        }

        public Task<InventoryItem> GetItemAsync(string itemId)
        {
            return Task.FromResult(
                Items
                    .Where(item => item.Id == itemId)
                    .FirstOrDefault());
        }

        // Boilerplate (entry point for the functions runtime)
        [FunctionName(nameof(InventoryEntity))]
        public static Task HandleEntityOperation(
            [EntityTrigger] IDurableEntityContext context)
        {
            return context.DispatchAsync<InventoryEntity>();
        }
    }
}
