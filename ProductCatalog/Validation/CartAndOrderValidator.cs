using FluentValidation;
using static ProductCatalog.DTOs.CartDtos;
using static ProductCatalog.DTOs.OrderDtos;

namespace ProductCatalog.Application.Validation
{
    public class AddToCartValidator : AbstractValidator<AddToCartDto>
    {
        public AddToCartValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Sku).NotEmpty().MaximumLength(64);
            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }
    public class UpdateCartItemValidator : AbstractValidator<UpdateCartItemDto>
    {
        public UpdateCartItemValidator()
        {
            RuleFor(x => x.CartItemId).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }

    public class CheckoutValidator : AbstractValidator<CheckoutDto>
    {
        public CheckoutValidator()
        {
            RuleFor(x => x.CartId).NotEmpty();
            RuleFor(x => x.CustomerName).NotEmpty().MaximumLength(200);
        }
    }
}