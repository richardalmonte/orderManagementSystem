using AddressBookService.Api.Contracts.V1.Requests;
using FluentValidation;

namespace AddressBookService.Api.Validators;

public class AddressUpdateRequestValidator : AbstractValidator<AddressUpdateRequest>
{
    public AddressUpdateRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.DeliveryAddressId)
            .NotEmpty();
    }
}