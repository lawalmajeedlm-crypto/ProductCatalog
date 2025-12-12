namespace ProductCatalog.DTOs
{
    public class CartDto
    {
        public record AddToCartRequest( Guid ProductId, int Quantity);
        public record CartItemResponse(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice, Decimal LineTotal);
        public record CartResponse(Guid Id, DateTime CreateUtc, decimal TotalAmount, IReadOnlyList<CartItemResponse> Items);
    }
}
