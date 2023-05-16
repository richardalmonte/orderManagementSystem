using FluentValidation;
using OrderService.Contracts.V1.Requests;

namespace OrderService.Validators;

public class OrderUpdateRequestValidator : AbstractValidator<OrderUpdateRequest>
{
    public OrderUpdateRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.DeliveryAddressId)
            .NotEmpty();
    }
}