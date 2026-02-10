namespace Application.Commands.Org.Setting.BankBranch.Validators
{
    using Application.Commands.Org.Setting.BankBranch.Commands;
    using Application.Validators;
    using Domain.Abstraction;
    using FluentValidation;
    using Utility;

    public class CreateBankBranchCommandValidator : Validator<CreateBankBranchCommand,  Entity.Model.BankBranch>
    {
        public CreateBankBranchCommandValidator(IRepository<Entity.Model.BankBranch> _Repository) : base(_Repository)
        {
            RuleFor(c => c.BankId)
            .NotEmpty().GreaterThanOrEqualTo(0).WithMessage("The bank field is required");
             
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");
             
            RuleFor(c => new { c.Name, c.BankId })           
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.BankId == Ob.BankId && c.Status != Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The bank branch name already exists")
            .OverridePropertyName(nameof(CreateBankBranchCommand.Name));
        }
    }
}