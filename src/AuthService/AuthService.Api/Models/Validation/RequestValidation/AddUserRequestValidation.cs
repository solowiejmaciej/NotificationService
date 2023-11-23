#region

using System.Text.RegularExpressions;
using AuthService.Api.Models.Requests.Add;
using AuthService.Domain.Interfaces;
using FluentValidation;

#endregion

namespace AuthService.Api.Models.Validation.RequestValidation;

public class AddUserRequestValidation : AbstractValidator<AddUserRequest>
{
    public AddUserRequestValidation(IUsersRepository repository)
    {
        RuleFor(u => u.Email)
            .EmailAddress()
            .NotEmpty()
            .Custom(
                (value, context) =>
                {
                    var emailInUse = repository.IsEmailInUse(value);

                    if (emailInUse) context.AddFailure("Email", "Already in use");
                });
        RuleFor(u => u.Password)
            .NotEmpty()
            .Equal(u => u.ConfirmPassword);
        RuleFor(u => u.ConfirmPassword)
            .NotEmpty();
        RuleFor(u => u.PhoneNumber)
            .NotEmpty()
            .Matches(new Regex(@"^\+?[1-9][0-9]{8,8}$")).WithMessage("PhoneNumber not valid");
    }
}