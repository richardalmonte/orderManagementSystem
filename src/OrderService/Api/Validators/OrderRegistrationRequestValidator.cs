using FluentValidation;
using OrderService.Contracts.V1.Requests;

namespace OrderService.Validators;

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