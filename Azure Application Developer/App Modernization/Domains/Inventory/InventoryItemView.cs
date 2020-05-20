namespace ECommerce.Domains.Inventory
{
    public sealed class InventoryItemView
    {
        public InventoryItemId Id { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int CustomerAvailableStock { get; set; }
    }
}