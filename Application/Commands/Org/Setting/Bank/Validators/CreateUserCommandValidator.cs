namespace Application.Commands.Org.Setting.Bank.Validators
{
    using Application.Commands.Org.Setting.Bank.Commands;
    using Application.Validators;
    using Domain.Abstraction;
    using FluentValidation;

    public class CreateBankCommandValidator : Validator<CreateBankCommand,  Domain.Entities.Bank>
    {
        public CreateBankCommandValidator(IRepository<Domain.Entities.Bank> _Repository) : base(_Repository)
        {            
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => new { c.Name})           
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Status != Domain.Enums.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The bank name already exists")
            .OverridePropertyName(nameof(CreateBankCommand.Name));
        }
    }
}