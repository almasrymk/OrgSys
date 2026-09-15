namespace Catalog.Application.Brands.Validators
{
    using FluentValidation;
    using OrgSys.SharedKernel;
    using Catalog.Application.Brands.Commands;

    public class UpdateBrandCommandValidator : Validator<UpdateBrandCommand, Catalog.Domain.Brand>
    {
        public UpdateBrandCommandValidator(IRepository<Catalog.Domain.Brand> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(100).WithMessage("The name must not exceed 100 characters");

            RuleFor(c => new { c.Name, c.Id })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Id != Ob.Id, cancellationToken))
            .WithMessage("The brand name already exists")
            .OverridePropertyName(nameof(UpdateBrandCommand.Name));
        }
    }
}
