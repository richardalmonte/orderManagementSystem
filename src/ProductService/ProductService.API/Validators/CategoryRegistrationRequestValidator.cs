using FluentValidation;
using ProductService.API.Contracts.V1.Requests;

namespace ProductService.API.Validators;

public class CategoryRegistrationRequestValidator : AbstractValidator<CategoryRegistrationRequest>
{
    public CategoryRegistrationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Name must not exceed 50 characters.")
            .WithErrorCode("invalid_name");
    }
}