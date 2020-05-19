namespace ECommerce.Domains.CustomerOrder
{
    public enum CustomerOrderStatus
    {
        Unknown,
        New,
        Submitted,
        InProcess,
        Backordered,
        Shipped,
        Canceled,
    }
}