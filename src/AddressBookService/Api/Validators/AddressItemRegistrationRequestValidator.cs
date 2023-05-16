using AddressBookService.Api.Contracts.V1.Requests;
using FluentValidation;

namespace AddressBookService.Api.Validators;

public class AddressItemRegistrationRequestValidator : AbstractValidator<AddressItemRegistrationRequest>
{
    public AddressItemRegistrationRequestValidator()
    {
        RuleFor(x => x.AddressId)
            .NotEmpty();

        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.Quantity)
            .GreaterThan(0);

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0);
    }
}