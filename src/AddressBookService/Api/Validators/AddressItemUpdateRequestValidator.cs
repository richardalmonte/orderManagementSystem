using AddressBookService.Api.Contracts.V1.Requests;
using FluentValidation;

namespace AddressBookService.Api.Validators;

public class AddressItemUpdateRequestValidator : AbstractValidator<AddressItemUpdateRequest>
{
    public AddressItemUpdateRequestValidator()
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