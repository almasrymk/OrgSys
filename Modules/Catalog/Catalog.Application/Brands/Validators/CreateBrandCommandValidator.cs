namespace Catalog.Application.Brands.Validators
{
    using FluentValidation;
    using OrgSys.SharedKernel;
    using Catalog.Application.Brands.Commands;

    public class CreateBrandCommandValidator : Validator<CreateBrandCommand, Catalog.Domain.Brand>
    {
        public CreateBrandCommandValidator(IRepository<Catalog.Domain.Brand> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(100).WithMessage("The name must not exceed 100 characters");

            RuleFor(c => c.Name)
            .MustAsync(async (Name, cancellationToken) => await NotAnyAsync(c => c.Name == Name, cancellationToken))
            .WithMessage("The brand name already exists")
            .OverridePropertyName(nameof(CreateBrandCommand.Name));
        }
    }
}
