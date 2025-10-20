using CarManagement.Application.Brands.Dtos;
using FluentValidation;

namespace CarManagement.Application.Brands.Validators;

public sealed class CreateBrandRequestValidator : AbstractValidator<CreateBrandRequest>
{
    public CreateBrandRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Country).NotEmpty().MaximumLength(100);
        RuleFor(x => x.FoundedYear).InclusiveBetween(1800, DateTime.UtcNow.Year);
    }
}

public sealed class UpdateBrandRequestValidator : AbstractValidator<UpdateBrandRequest>
{
    public UpdateBrandRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Country).NotEmpty().MaximumLength(100);
        RuleFor(x => x.FoundedYear).InclusiveBetween(1800, DateTime.UtcNow.Year);
    }
}
