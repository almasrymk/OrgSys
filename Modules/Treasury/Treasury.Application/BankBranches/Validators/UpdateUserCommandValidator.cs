namespace Treasury.Application.BankBranches.Validators
{
    using FluentValidation;
    using OrgSys.SharedKernel;
    using Treasury.Application.BankBranches.Commands;

    public class UpdateBankBranchCommandValidator : Validator<UpdateBankBranchCommand, Treasury.Domain.BankBranch>
    {
        public UpdateBankBranchCommandValidator(IRepository<Treasury.Domain.BankBranch> _Repository) : base(_Repository)
        {
            RuleFor(c => c.BankId)
            .NotEmpty().GreaterThanOrEqualTo(0).WithMessage("The bank field is required");

            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");
             
            RuleFor(c => new { c.Name, c.Id, c.BankId })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Id != Ob.Id && c.BankId == Ob.BankId && c.Status != OrgSys.SharedKernel.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The bank branch name already exists")
            .OverridePropertyName(nameof(CreateBankBranchCommand.Name));
        }
    }
}