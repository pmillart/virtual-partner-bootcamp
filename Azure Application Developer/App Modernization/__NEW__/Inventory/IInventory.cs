using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Domain.Inventory
{
    public interface IInventory
    {
        Task<int> AddStockAsync(string itemId);
        Task<int> RemoveStockAsync(string itemId);
        Task<bool> IsItemInInventoryAsync(string itemId);
        Task<bool> CreateItemAsync(InventoryItem item);
        Task<InventoryItem> GetItemAsync(string itemId);
    }
}
