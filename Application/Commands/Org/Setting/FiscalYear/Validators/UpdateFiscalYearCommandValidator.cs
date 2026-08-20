namespace Application.Commands.Org.Setting.FiscalYear.Validators
{
    using FluentValidation;
    using Domain.Abstraction;
    using Application.Validators;
    using Application.Commands.Org.Setting.FiscalYear.Commands;

    public class UpdateFiscalYearCommandValidator : Validator<UpdateFiscalYearCommand, Domain.Entities.FiscalYear>
    {
        public UpdateFiscalYearCommandValidator(IRepository<Domain.Entities.FiscalYear> _Repository) : base(_Repository)
        {
            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required");

            RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("The name must not exceed 150 characters");

            RuleFor(c => c.EndDate)
            .GreaterThan(c => c.StartDate).WithMessage("The end date must be after the start date");

            RuleFor(c => new { c.Name, c.Id })
            .MustAsync(async (Ob, cancellationToken) => await NotAnyAsync(c => c.Name == Ob.Name && c.Id != Ob.Id && c.Status != Domain.Enums.Status.Deleted && c.Hide != true, cancellationToken))
            .WithMessage("The fiscal year name already exists")
            .OverridePropertyName(nameof(CreateFiscalYearCommand.Name));
        }
    }
}
