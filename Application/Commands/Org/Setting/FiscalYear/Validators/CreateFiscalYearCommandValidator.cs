namespace Application.Commands.Org.Setting.FiscalYear.Validators
{
    using Application.Commands.Org.Setting.FiscalYear.Commands;
    using Application.Validators;
    using Domain.Abstraction;
    using FluentValidation;

    public class CreateFiscalYearCommandValidator : Validator<CreateFiscalYearCommand, Domain.Entities.FiscalYear>
    {
        public CreateFiscalYearCommandValidator(IRepository<Domain.Entities.FiscalYear> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => c.EndDate)
            .GreaterThan(c => c.StartDate).WithMessage("The end date must be after the start date");

            RuleFor(c => new { c.Name })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Status != Domain.Enums.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The fiscal year name already exists")
            .OverridePropertyName(nameof(CreateFiscalYearCommand.Name));
        }
    }
}
