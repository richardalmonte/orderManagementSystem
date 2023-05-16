using FluentValidation;
using OrderService.Contracts.V1.Requests;

namespace OrderService.Validators;

public class OrderItemRegistrationRequestValidator : AbstractValidator<OrderItemRegistrationRequest>
{
    public OrderItemRegistrationRequestValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty();

        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.Quantity)
            .GreaterThan(0);

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0);
    }
}