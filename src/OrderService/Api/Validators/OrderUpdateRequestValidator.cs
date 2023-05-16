using FluentValidation;
using OrderService.Api.Contracts.V1.Requests;

namespace OrderService.Api.Validators;

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