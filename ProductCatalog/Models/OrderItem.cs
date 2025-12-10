using ProductCatalog.Entities;

namespace ProductCatalog.Models
{
    public sealed class OrderItem : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UintPrice { get; set; }
        public decimal LineTotal => Quantity * UintPrice;

        public Order Order { get; set; } = default!;
        public Product Product { get; set; } = default!;
    }
}
