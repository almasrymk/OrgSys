namespace Accounting.Application.FiscalYears.Validators
{
    using Accounting.Application.FiscalYears.Commands;
    using OrgSys.SharedKernel;
    using FluentValidation;

    public class CreateFiscalYearCommandValidator : Validator<CreateFiscalYearCommand, Accounting.Domain.FiscalYear>
    {
        public CreateFiscalYearCommandValidator(IRepository<Accounting.Domain.FiscalYear> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => c.EndDate)
            .GreaterThan(c => c.StartDate).WithMessage("The end date must be after the start date");

            RuleFor(c => new { c.Name })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Status != OrgSys.SharedKernel.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The fiscal year name already exists")
            .OverridePropertyName(nameof(CreateFiscalYearCommand.Name));
        }
    }
}
