using ProductCatalog.Models;

namespace ProductCatalog.Entities;

public sealed class Order : BaseEntity
{
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Placed";

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
