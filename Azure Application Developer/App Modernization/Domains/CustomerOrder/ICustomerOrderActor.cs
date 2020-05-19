using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Domains.CustomerOrder
{
    public interface ICustomerOrderActor
    {
        Task<string> GetOrderStatusAsStringAsync();

        Task SubmitOrderAsync(IEnumerable<CustomerOrderItem> orderList);
    }
}