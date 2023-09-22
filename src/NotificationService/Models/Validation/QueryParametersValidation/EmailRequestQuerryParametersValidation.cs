using FluentValidation;
using NotificationService.Entities;
using NotificationService.Models.QueryParameters.Create;

namespace NotificationService.Models.Validation.QueryParametersValidation
{
    public class EmailRequestQuerryParametersValidation : AbstractValidator<CreateEmailRequestQueryParameters>
    {
        public EmailRequestQuerryParametersValidation(NotificationDbContext dbContext)
        {
            RuleFor(e => e.UserId)
                .NotEmpty();
        }
    }
}