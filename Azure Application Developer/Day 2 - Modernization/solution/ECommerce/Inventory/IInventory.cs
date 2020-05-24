using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Domain.Inventory
{
    public interface IInventory
    {
        void AddStock(string itemId);
        Task<bool> RemoveStockAsync(string itemId);
        Task<bool> IsItemInInventory(string itemId);
        Task<bool> CreateItemAsync(InventoryItem item);
        Task<InventoryItem> GetItemAsync(string itemId);
    }
}
