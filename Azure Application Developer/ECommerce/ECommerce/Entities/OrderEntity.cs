using ECommerce.Domain.Order;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Entities
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class OrderEntity : IOrder
    {
        [JsonProperty]
        public List<OrderItem> Orders { get; set; } = new List<OrderItem>();

        public Task<bool> AddAsync(OrderItem order)
        {
            Orders.Add(order);
            return Task.FromResult(true);
        }

        public void Remove(DateTime timestamp)
        {
            Orders.RemoveAll(order => order.Timestamp == timestamp);
        }

        public Task<List<OrderItem>> Get()
        {
            return Task.FromResult(Orders);
        }

        // Boilerplate (entry point for the functions runtime)
        [FunctionName(nameof(OrderEntity))]
        public static Task HandleEntityOperation([EntityTrigger] IDurableEntityContext context)
        {
            return context.DispatchAsync<OrderEntity>();
        }
    }
}