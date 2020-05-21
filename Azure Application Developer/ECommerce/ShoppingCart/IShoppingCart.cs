using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Domain.ShoppingCart
{
    public interface IShoppingCart
    {
        Task AddItemAsync(string itemId);
        Task RemoveItemAsync(string itemId);
        Task<IEnumerable<string>> GetItemsAsync();
    }
}
