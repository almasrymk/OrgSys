namespace Catalog.Application.Categories.Validators
{
    using FluentValidation;
    using OrgSys.SharedKernel;
    using Catalog.Application.Categories.Commands;

    public class CreateClassificationCommandValidator : Validator<CreateClassificationCommand,  Catalog.Domain.Classification>
    {
        public CreateClassificationCommandValidator(IRepository<Catalog.Domain.Classification> _Repository) : base(_Repository)
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
