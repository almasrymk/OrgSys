namespace Catalog.Application.Units.Validators
{
    using FluentValidation;
    using OrgSys.SharedKernel;
    using Catalog.Application.Units.Commands;

    public class CreateUnitCommandValidator : Validator<CreateUnitCommand,  Catalog.Domain.Unit>
    {
        public CreateUnitCommandValidator(IRepository<Catalog.Domain.Unit> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => c.Name)           
            .MustAsync(async (Name, cancellationToken) => await NotAnyAsync(c => c.Name == Name, cancellationToken))
            .WithMessage("The unit name already exists")
            .OverridePropertyName(nameof(CreateUnitCommand.Name));
        }
    }
}
