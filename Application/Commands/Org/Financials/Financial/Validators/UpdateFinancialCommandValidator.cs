namespace Application.Commands.Org.Financials.Financial.Validators
{
    using Application.Commands.Org.Financials.Financial.Commands;
    using Application.Validators;
    using Domain.Abstraction;
    using Domain.Entities;
    using Domain.Enums;
    using FluentValidation;

    /// <summary>Same Opening Balance uniqueness rule as CreateFinancialCommandValidator, excluding the row being edited.</summary>
    public class UpdateFinancialCommandValidator : Validator<UpdateFinancialCommand, Domain.Entities.Financial>
    {
        public UpdateFinancialCommandValidator(
            IRepository<Domain.Entities.Financial> repository,
            IRepository<FiscalYear> fiscalYearRepository) : base(repository)
        {
            When(c => c.FinancialTypeId == (long)FinancialTransactionType.OpeningBalance, () =>
            {
                RuleFor(c => c.FinancialAccountId)
                    .NotNull().GreaterThan(0)
                    .WithMessage("Financial account is required for an Opening Balance.");

                RuleFor(c => c.Amount)
                    .GreaterThan(0).WithMessage("Opening Balance amount must be greater than zero.");

                RuleFor(c => c)
                    .MustAsync(async (command, cancellationToken) =>
                    {
                        var fiscalYear = (await fiscalYearRepository.GetListByFilterAsync(
                            e => e.StartDate.Date <= command.Date.Date && e.EndDate.Date >= command.Date.Date))
                            ?.FirstOrDefault();
                        if (fiscalYear is null)
                            return true;

                        return await NotAnyAsync(e =>
                            e.Id != command.Id &&
                            e.FinancialTypeId == (long)FinancialTransactionType.OpeningBalance &&
                            e.FinancialAccountId == command.FinancialAccountId &&
                            e.Status != Status.Deleted && e.Status != Status.Cancel && e.Status != Status.Reversed &&
                            e.Hide != true &&
                            e.Date.Date >= fiscalYear.StartDate.Date && e.Date.Date <= fiscalYear.EndDate.Date,
                            cancellationToken);
                    })
                    .WithMessage("An Opening Balance already exists for this Financial Account in this fiscal year.")
                    .OverridePropertyName(nameof(UpdateFinancialCommand.FinancialAccountId));
            });
        }
    }
}
