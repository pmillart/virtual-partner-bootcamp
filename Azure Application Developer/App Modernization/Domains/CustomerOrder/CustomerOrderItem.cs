using ECommerce.Domains.Inventory;
using System;
using System.Runtime.Serialization;

namespace ECommerce.Domains.CustomerOrder
{
    public sealed class CustomerOrderItem
    {
        public CustomerOrderItem(InventoryItemId itemId, int quantity)
        {
            this.ItemId = itemId;
            this.Quantity = quantity;
            this.FulfillmentRemaining = quantity;
        }

        public InventoryItemId ItemId { get; set; }

        public int Quantity { get; set; }

        public int FulfillmentRemaining { get; set; }

        public override string ToString()
        {
            return String.Format("ID: {0}, Quantity: {1}, Fulfillment Remaing: {2}", this.ItemId, this.Quantity, this.FulfillmentRemaining);
        }
    }
}