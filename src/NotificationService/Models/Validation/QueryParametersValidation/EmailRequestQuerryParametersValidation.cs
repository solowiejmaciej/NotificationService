#region

using FluentValidation;
using NotificationService.Entities;
using NotificationService.Models.QueryParameters.Create;

#endregion

namespace NotificationService.Models.Validation.QueryParametersValidation;

public class EmailRequestQuerryParametersValidation : AbstractValidator<CreateEmailRequestQueryParameters>
{
    public EmailRequestQuerryParametersValidation(NotificationDbContext dbContext)
    {
        RuleFor(e => e.UserId)
            .NotEmpty();
    }
}