namespace MasterData.Application.Classifications.Validators
{
    using FluentValidation;
    using OrgSys.SharedKernel;
    using MasterData.Application.Classifications.Commands;

    public class CreateClassificationCommandValidator : Validator<CreateClassificationCommand,  MasterData.Domain.Classification>
    {
        public CreateClassificationCommandValidator(IRepository<MasterData.Domain.Classification> _Repository) : base(_Repository)
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
