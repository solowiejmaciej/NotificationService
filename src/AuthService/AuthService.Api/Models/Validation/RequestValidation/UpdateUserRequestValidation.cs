using System.Text.RegularExpressions;
using AuthService.Domain.Interfaces;
using AuthService.Models.Requests.Update;
using FluentValidation;

namespace AuthService.Api.Models.Validation.RequestValidation
{
    public class UpdateUserRequestValidation : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidation(IUsersRepository repository)
        {
            RuleFor(u => u.Email)
                .EmailAddress()
                .Custom(
                    (value, context) =>
                    {
                        var emailInUse = repository.IsEmailInUse(value);

                        if (emailInUse)
                        {
                            context.AddFailure("Email", "Already in use");
                        }
                    });
            RuleFor(u => u.PhoneNumber)
                .Matches(new Regex(@"^\+?[1-9][0-9]{8,8}$")).WithMessage("PhoneNumber not valid");
            RuleFor(u => u.Firstname)
                .MinimumLength(2)
                .MaximumLength(25);
            RuleFor(u => u.Surname)
                .MinimumLength(2)
                .MaximumLength(25);
            RuleFor(u => u.DeviceId)
                .MinimumLength(10)
                .MaximumLength(100);
        }
    }
}