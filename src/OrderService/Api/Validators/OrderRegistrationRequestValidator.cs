using FluentValidation;
using OrderService.Api.Contracts.V1.Requests;

namespace OrderService.Api.Validators;

public class OrderRegistrationRequestValidator : AbstractValidator<OrderRegistrationRequest>
{
    public OrderRegistrationRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.DeliveryAddressId)
            .NotEmpty();
    }
}