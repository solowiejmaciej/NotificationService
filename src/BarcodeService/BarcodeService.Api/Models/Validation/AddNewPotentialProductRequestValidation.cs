using System.Text.RegularExpressions;
using BarcodeService.Api.Models.Requests;
using FluentValidation;

namespace BarcodeService.Api.Models.Validation;

public class AddNewPotentialProductRequestValidation : AbstractValidator<AddNewPotentialProductRequest>
{
    public AddNewPotentialProductRequestValidation()
    {
        RuleFor(p => p.Ean)
            .NotEmpty()
            .Matches(new Regex(@"[0-9]")).WithMessage("Barcode must be a number");
        RuleFor(p => p.Name)
            .NotEmpty();
        RuleFor(p => p.Price)
            .NotEmpty();
    }
}