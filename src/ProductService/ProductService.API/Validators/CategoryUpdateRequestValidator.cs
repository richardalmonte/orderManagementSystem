using FluentValidation;
using ProductService.API.Contracts.V1.Requests;

namespace ProductService.API.Validators;

public class CategoryUpdateRequestValidator : AbstractValidator<CategoryUpdateRequest>
{
    public CategoryUpdateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Name must not exceed 50 characters.")
            .WithErrorCode("invalid_name");
    }
}