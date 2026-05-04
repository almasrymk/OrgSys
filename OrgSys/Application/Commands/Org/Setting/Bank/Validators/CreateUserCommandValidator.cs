namespace Application.Commands.Org.Setting.Bank.Validators
{
    using Application.Commands.Org.Setting.Bank.Commands;
    using Application.Validators;
    using Domain.Abstraction;
    using FluentValidation;
    using Utility;

    public class CreateBankCommandValidator : Validator<CreateBankCommand,  Entity.Model.Bank>
    {
        public CreateBankCommandValidator(IRepository<Entity.Model.Bank> _Repository) : base(_Repository)
        {            
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => new { c.Name})           
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Status != Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The bank name already exists")
            .OverridePropertyName(nameof(CreateBankCommand.Name));
        }
    }
}