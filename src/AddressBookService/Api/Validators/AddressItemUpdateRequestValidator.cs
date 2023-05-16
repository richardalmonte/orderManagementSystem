using AddressBookService.Api.Contracts.V1.Requests;
using FluentValidation;

namespace AddressBookService.Api.Validators;

public class AddressItemUpdateRequestValidator : AbstractValidator<AddressItemUpdateRequest>
{
    public AddressItemUpdateRequestValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(100);
    }
}