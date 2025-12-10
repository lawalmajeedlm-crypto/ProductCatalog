namespace ProductCatalog.DTOs
{
    public class ProductDto
    {
        public record CreateProductRequest(
            string Name,
            string? Description,
            decimal Price,
            int StockQuantity
        );

        public record UpdateProductRequest(
                Guid Id,
                string Name,
                string? Description,
                decimal Price,
                int StockQuantity
            );
        public record ProductResponse(
            Guid Id,
            string Name,
            string? Description,
            decimal Price,
            int StockQuantity,
            DateTime CreateUtc,
            DateTime? UpdateUtc,
            string? CreatedBy,
            string? UpdatedBy
        );
    }
}
