using ECommerce.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Domains.Inventory
{
    public interface IInventoryService
    {
        Task<int> AddStockAsync(InventoryItemId itemId, int quantity);
        Task<int> RemoveStockAsync(InventoryItemId itemId, int quantity/*, CustomerOrderActorMessageId messageId*/);
        Task<bool> IsItemInInventoryAsync(InventoryItemId itemId, CancellationToken cancellationToken);
        Task<IEnumerable<InventoryItemView>> GetCustomerInventoryAsync(CancellationToken cancellationToken);
        Task<bool> CreateInventoryItemAsync(InventoryItem item);
    }
}