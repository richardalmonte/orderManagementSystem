using AddressBookService.Api.Contracts.V1.Requests;
using FluentValidation;

namespace AddressBookService.Api.Validators;

public class AddressRegistrationRequestValidator : AbstractValidator<AddressRegistrationRequest>
{
    public AddressRegistrationRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Street)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(100);
    }
}