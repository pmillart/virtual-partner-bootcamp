using ECommerce.Domain.ShoppingCart;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Entities
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ShoppingCartEntity : IShoppingCart
    {
        [JsonProperty]
        public List<string> Items { get; set; } = new List<string>();
        
        public Task AddItemAsync(string itemId)
        {
            this.Items.Add(itemId);
            return Task.FromResult(0);
        }

        public Task RemoveItemAsync(string itemId)
        {
            if (this.Items.Contains(itemId))
                this.Items.Remove(itemId);
            return Task.FromResult(0);
        }

        public Task<IEnumerable<string>> GetItemsAsync()
        {
            return Task.FromResult<IEnumerable<string>>(this.Items);
        }

        // Boilerplate (entry point for the functions runtime)
        [FunctionName(nameof(ShoppingCartEntity))]
        public static Task HandleEntityOperation([EntityTrigger] IDurableEntityContext context)
        {
            return context.DispatchAsync<ShoppingCartEntity>();
        }
    }
}