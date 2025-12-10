namespace ProductCatalog.Models
{
    public abstract class BaseEntity
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreateUtc { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateUtc { get; set; }
        public string? CreatedBy  { get; set; }
        public string? UpdatedBy { get; set; }

    }
}
