namespace Accounting.Application.Accounts.Validators
{
    using Accounting.Application.Accounts.Commands;
    using OrgSys.SharedKernel;
    using FluentValidation;
    using System.Xml.Linq;

    public class CreateAccountCommandValidator : Validator<CreateAccountCommand,  Accounting.Domain.Account>
    {
        public CreateAccountCommandValidator(IRepository<Accounting.Domain.Account> _Repository) : base(_Repository)
        {
            RuleFor(c => c.AccountTypeId)
            .NotEmpty().GreaterThanOrEqualTo(0).WithMessage("The account type field is required");

            RuleFor(c => c.Code)
            .NotEmpty().WithMessage("The code field is required");

            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");
             
            RuleFor(c => new { c.Code , c.TypeId })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Code == Ob.Code && c.TypeId == Ob.TypeId && c.Status != OrgSys.SharedKernel.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The account code already exists")
            .OverridePropertyName(nameof(CreateAccountCommand.Code));

            RuleFor(c => new { c.Name, c.TypeId })           
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.TypeId == Ob.TypeId && c.Status != OrgSys.SharedKernel.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The account name already exists")
            .OverridePropertyName(nameof(CreateAccountCommand.Name));
        }
    }
}