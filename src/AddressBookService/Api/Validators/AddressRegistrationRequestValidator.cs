using AddressBookService.Api.Contracts.V1.Requests;
using FluentValidation;

namespace AddressBookService.Api.Validators;

public class AddressRegistrationRequestValidator : AbstractValidator<AddressRegistrationRequest>
{
    public AddressRegistrationRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.DeliveryAddressId)
            .NotEmpty();
    }
}