namespace Treasury.Application.Banks.Validators
{
    using Treasury.Application.Banks.Commands;
    using OrgSys.SharedKernel;
    using FluentValidation;

    public class CreateBankCommandValidator : Validator<CreateBankCommand,  Treasury.Domain.Bank>
    {
        public CreateBankCommandValidator(IRepository<Treasury.Domain.Bank> _Repository) : base(_Repository)
        {            
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => new { c.Name})           
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Status != OrgSys.SharedKernel.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The bank name already exists")
            .OverridePropertyName(nameof(CreateBankCommand.Name));
        }
    }
}