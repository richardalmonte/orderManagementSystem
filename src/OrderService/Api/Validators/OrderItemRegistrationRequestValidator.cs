using FluentValidation;
using OrderService.Api.Contracts.V1.Requests;

namespace OrderService.Api.Validators;

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