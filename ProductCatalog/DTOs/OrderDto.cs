using static ProductCatalog.DTOs.OrderDto;

namespace ProductCatalog.DTOs
{
    public class OrderDto
    {
        public record OrderItemRequest(
            Guid ProductId,
            int Quantity
        );

        public record PlaceOrderRequest(
            IReadOnlyList<OrderItemRequest> Items
            );
        public record OrderLineResponse(
            Guid Id,
            DateTime CreateUtc,
            string Status,
            decimal TotalAmount,
            IReadOnlyList<OrderLineResponse> Items
            );
    }
}
