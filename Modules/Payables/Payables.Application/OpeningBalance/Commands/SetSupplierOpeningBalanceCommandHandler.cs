namespace Payables.Application.OpeningBalance.Commands
{
    using Accounting.Contracts.Accounts;
    using Accounting.Contracts.Postings;
    using Administration.Contracts.Preferences;
    using MasterData.Contracts.Currencies;
    using MediatR;
    using OrgSys.SharedKernel;
    using Parties.Contracts.Dealers;
    using Payables.Domain;
    using Payables.Domain.Repositories;
    using System.Net;

    /// <summary>Records/updates a supplier's opening payable balance by delegating the actual
    /// Journal/JournalItem mechanics to Accounting.Contracts.Postings.SetOpeningBalanceLineCommand —
    /// this module only resolves which supplier/account/clearing-account preference apply (see the
    /// Accounting DDD cleanup report: Payables must not construct Journal/JournalItem directly).
    /// Mirrors <c>SetCustomerOpeningBalanceCommandHandler</c> on the AR side.</summary>
    public sealed record SetSupplierOpeningBalanceCommand(
        long DealerId,
        long FiscalYearId,
        decimal Amount,
        bool SupplierIsDebit,
        long CreateUserId) : ICommand, ICreateCommand<Result>;

    public sealed class SetSupplierOpeningBalanceCommandHandler(
        ISender _Sender,
        IPayableAccountValidator _Validator,
        IReceivableAccountValidator _AccountValidator,
        IPayableRepository _PayableRepository,
        IUnitOfWork _UnitOfWork) : ICommandHandler<SetSupplierOpeningBalanceCommand>
    {
        public async Task<Result> Handle(SetSupplierOpeningBalanceCommand request, CancellationToken cancellationToken)
        {
            if (request.Amount <= 0)
                return BadRequest("Amount must be greater than zero.");

            var (dealer, account, supplierErrors) = await _Validator.ValidateSupplierAsync(request.DealerId, cancellationToken);
            if (supplierErrors.Count > 0)
                return new Result(HttpStatusCode.BadRequest, supplierErrors);

            var clearingAccountValue = (await _Sender.Send(
                new GetPreferenceValueQuery("Dealer", (long)DealerType.Supplier, "OpeningBalanceClearingAccountId"), cancellationToken)).Response;
            var clearingAccountId = long.TryParse(clearingAccountValue, out var cid) ? cid : 0;
            if (clearingAccountId <= 0)
                return BadRequest("Opening balance clearing account is not configured (OpeningBalanceClearingAccountId preference).");

            var (_, clearingErrors) = await _AccountValidator.ValidateAccountAsync(clearingAccountId, cancellationToken);
            if (clearingErrors.Count > 0)
                return new Result(HttpStatusCode.BadRequest, clearingErrors);

            var debit = request.SupplierIsDebit ? request.Amount : 0;
            var credit = request.SupplierIsDebit ? 0 : request.Amount;

            var glResult = await _Sender.Send(new SetOpeningBalanceLineCommand(
                request.FiscalYearId, account!.Id, debit, credit, $"Opening balance — {dealer!.Name}", clearingAccountId, request.CreateUserId),
                cancellationToken);
            if (glResult.StatusCode != HttpStatusCode.OK)
                return glResult;

            // AP open-item side of the opening balance — mirrors Receivables'
            // SetCustomerOpeningBalanceCommandHandler exactly, mirrored for sign: a supplier's
            // opening balance is an AP obligation when it posts as Credit (SupplierIsDebit == false)
            // — a Debit opening balance is a prepayment/on-account credit from the supplier, not a
            // liability, so no Payable is created for it (the "supplier advance" concept itself is
            // out of scope, deferred).
            if (!request.SupplierIsDebit
                && !await _PayableRepository.ExistsForSourceDocumentAsync(SourceDocumentType.OpeningBalance, request.FiscalYearId, request.DealerId, cancellationToken))
            {
                var currency = (await _Sender.Send(new GetDefaultCurrencyQuery(), cancellationToken)).Response;
                var openingDate = DateTime.Now;

                var payable = Payable.Create(
                    supplierId: request.DealerId,
                    sourceDocumentType: SourceDocumentType.OpeningBalance,
                    sourceDocumentId: request.FiscalYearId,
                    sourceDocumentNumber: null,
                    documentDate: openingDate,
                    dueDate: openingDate,
                    currencyId: currency?.Id ?? 0,
                    rate: currency?.Rate ?? 1,
                    originalAmount: request.Amount,
                    createUserId: request.CreateUserId,
                    createDate: openingDate);

                await _PayableRepository.AddAsync(payable, cancellationToken);
                await _UnitOfWork.SaveChangeAsync(cancellationToken);
            }

            return glResult;
        }

        private static Result BadRequest(string message) => new(HttpStatusCode.BadRequest, [new Error(message)]);
    }
}
