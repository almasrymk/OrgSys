namespace Treasury.Application.Financials.Validators
{
    using Accounting.Contracts.Postings;
    using MediatR;
    using Treasury.Application.Financials.Commands;
    using OrgSys.SharedKernel;
    using FluentValidation;

    /// <summary>
    /// Only Opening Balance Financial rows get validated here — Receipt/Payment/Deposit/etc. keep their
    /// existing (unvalidated-at-this-layer) Save flow via PostFinancialTransactionCommand unchanged.
    /// </summary>
    public class CreateFinancialCommandValidator : Validator<CreateFinancialCommand, Treasury.Domain.Financial>
    {
        public CreateFinancialCommandValidator(
            IRepository<Treasury.Domain.Financial> repository,
            ISender sender) : base(repository)
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
                        var fiscalYear = (await sender.Send(new GetFiscalYearForDateQuery(command.Date), cancellationToken)).Response;
                        if (fiscalYear is null)
                            return true; // no fiscal year for this date — reported by the posting step instead

                        return await NotAnyAsync(e =>
                            e.FinancialTypeId == (long)FinancialTransactionType.OpeningBalance &&
                            e.FinancialAccountId == command.FinancialAccountId &&
                            e.Status != Status.Deleted && e.Status != Status.Cancel && e.Status != Status.Reversed &&
                            e.Hide != true &&
                            e.Date.Date >= fiscalYear.StartDate.Date && e.Date.Date <= fiscalYear.EndDate.Date,
                            cancellationToken);
                    })
                    .WithMessage("An Opening Balance already exists for this Financial Account in this fiscal year.")
                    .OverridePropertyName(nameof(CreateFinancialCommand.FinancialAccountId));
            });
        }
    }
}
