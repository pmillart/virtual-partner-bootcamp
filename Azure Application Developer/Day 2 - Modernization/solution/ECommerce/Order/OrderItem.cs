using System;
using System.Collections.Generic;

namespace ECommerce.Domain.Order
{
    public class OrderItem
    {
        public string UserId { get; set; }

        public DateTime Timestamp { get; set; }

        public IEnumerable<string> Details { get; set; }
    }
}
