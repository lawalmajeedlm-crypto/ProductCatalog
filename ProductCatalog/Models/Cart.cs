namespace ProductCatalog.Models
{
    public sealed class Cart : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal TotalAmount { get; set; }
    }
}
