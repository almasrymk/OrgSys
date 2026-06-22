namespace Application.Commands.Org.Setting.Unit.Validators
{
    using FluentValidation;
    using Domain.Abstraction;
    using Application.Validators;
    using Application.Commands.Org.Setting.Unit.Commands;

    public class CreateUnitCommandValidator : Validator<CreateUnitCommand,  Domain.Entities.Unit>
    {
        public CreateUnitCommandValidator(IRepository<Domain.Entities.Unit> _Repository) : base(_Repository)
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