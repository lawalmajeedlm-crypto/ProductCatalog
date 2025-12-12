using Microsoft.AspNetCore.Identity;
using ProductCatalog.Entities;


namespace ProductCatalog.Models
{
    public sealed class User : IdentityUser<Guid>
    {
           public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = "Customer";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();

    }
}
