using ProductCatalog.Models;

namespace ProductCatalog.Entities;

public sealed class Order : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Placed";

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
