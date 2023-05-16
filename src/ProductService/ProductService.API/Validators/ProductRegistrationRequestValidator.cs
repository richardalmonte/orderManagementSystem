using FluentValidation;
using ProductService.API.Contracts.V1.Requests;

namespace ProductService.API.Validators;

public class ProductRegistrationRequestValidator : AbstractValidator<ProductRegistrationRequest>
{
    public ProductRegistrationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Name must not exceed 50 characters.")
            .WithErrorCode("invalid_name");


        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.")
            .WithErrorCode("invalid_description")
            .MinimumLength(10)
            .WithMessage("Description must be at least 10 characters.")
            .WithErrorCode("invalid_description");


        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("CategoryId must not be empty.")
            .WithErrorCode("invalid_category_id");
    }
}