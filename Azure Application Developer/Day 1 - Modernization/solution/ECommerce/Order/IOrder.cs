using System;
using System.Threading.Tasks;

namespace ECommerce.Domain.Order
{
    public interface IOrder
    {
        Task<bool> AddAsync(OrderItem chirp);

        void Remove(DateTime timestamp);
    }
}
