namespace Application.Commands.Org.Setting.Classification.Validators
{
    using FluentValidation;
    using Domain.Abstraction;
    using Application.Validators;
    using Application.Commands.Org.Setting.Classification.Commands;

    public class CreateClassificationCommandValidator : Validator<CreateClassificationCommand,  Domain.Entities.Classification>
    {
        public CreateClassificationCommandValidator(IRepository<Domain.Entities.Classification> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => c.Name)           
            .MustAsync(async (Name, cancellationToken) => await NotAnyAsync(c => c.Name == Name, cancellationToken))
            .WithMessage("The classification name already exists")
            .OverridePropertyName(nameof(CreateClassificationCommand.Name));
        }
    }
}