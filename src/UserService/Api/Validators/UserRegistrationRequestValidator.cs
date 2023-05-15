using FluentValidation;
using UserService.Contracts.V1.Requests;

namespace UserService.Validators;

public class UserRegistrationRequestValidator : AbstractValidator<UserRegistrationRequest>
{
    public UserRegistrationRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}