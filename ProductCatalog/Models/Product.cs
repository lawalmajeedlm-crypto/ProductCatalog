using ProductCatalog.Models;

namespace ProductCatalog.Entities;

public sealed class Product : BaseEntity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }

}
