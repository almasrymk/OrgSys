namespace Treasury.Application.BankBranches.Validators
{
    using Treasury.Application.BankBranches.Commands;
    using OrgSys.SharedKernel;
    using FluentValidation;

    public class CreateBankBranchCommandValidator : Validator<CreateBankBranchCommand,  Treasury.Domain.BankBranch>
    {
        public CreateBankBranchCommandValidator(IRepository<Treasury.Domain.BankBranch> _Repository) : base(_Repository)
        {
            RuleFor(c => c.BankId)
            .NotEmpty().GreaterThanOrEqualTo(0).WithMessage("The bank field is required");
             
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");
             
            RuleFor(c => new { c.Name, c.BankId })           
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.BankId == Ob.BankId && c.Status != OrgSys.SharedKernel.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The bank branch name already exists")
            .OverridePropertyName(nameof(CreateBankBranchCommand.Name));
        }
    }
}