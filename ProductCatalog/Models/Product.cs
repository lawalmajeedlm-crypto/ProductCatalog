using ProductCatalog.Models;

namespace ProductCatalog.Entities;

public sealed class Product : BaseEntity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public ICollection<Picture> Pictures { get; set; } = new List<Picture>();
    public ICollection<Order> Orders { get; set; }= new List<Order>();
    public ICollection<Cart> Carts { get; set; }= new List<Cart>();
    public DateTime? UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
  
}
