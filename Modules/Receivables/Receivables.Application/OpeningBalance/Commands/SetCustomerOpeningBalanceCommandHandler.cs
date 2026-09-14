namespace Receivables.Application.OpeningBalance.Commands
{
    using Accounting.Contracts.Accounts;
    using Accounting.Contracts.Postings;
    using Administration.Contracts.Preferences;
    using MasterData.Contracts.Currencies;
    using MediatR;
    using OrgSys.SharedKernel;
    using Parties.Contracts.Dealers;
    using Receivables.Domain;
    using Receivables.Domain.Repositories;
    using System.Net;

    /// <summary>Records/updates a customer's opening receivable balance by delegating the actual
    /// Journal/JournalItem mechanics to Accounting.Contracts.Postings.SetOpeningBalanceLineCommand —
    /// this module only resolves which customer/account/clearing-account preference apply (see the
    /// Accounting DDD cleanup report: Receivables must not construct Journal/JournalItem directly).</summary>
    public sealed record SetCustomerOpeningBalanceCommand(
        long DealerId,
        long FiscalYearId,
        decimal Amount,
        bool CustomerIsDebit,
        long CreateUserId) : ICommand, ICreateCommand<Result>;

    public sealed class SetCustomerOpeningBalanceCommandHandler(
        ISender _Sender,
        IReceivableAccountValidator _Validator,
        IReceivableRepository _ReceivableRepository,
        IUnitOfWork _UnitOfWork) : ICommandHandler<SetCustomerOpeningBalanceCommand>
    {
        public async Task<Result> Handle(SetCustomerOpeningBalanceCommand request, CancellationToken cancellationToken)
        {
            if (request.Amount <= 0)
                return BadRequest("Amount must be greater than zero.");

            var (dealer, account, customerErrors) = await _Validator.ValidateCustomerAsync(request.DealerId, cancellationToken);
            if (customerErrors.Count > 0)
                return new Result(HttpStatusCode.BadRequest, customerErrors);

            var clearingAccountValue = (await _Sender.Send(
                new GetPreferenceValueQuery("Dealer", (long)DealerType.Client, "OpeningBalanceClearingAccountId"), cancellationToken)).Response;
            var clearingAccountId = long.TryParse(clearingAccountValue, out var cid) ? cid : 0;
            if (clearingAccountId <= 0)
                return BadRequest("Opening balance clearing account is not configured (OpeningBalanceClearingAccountId preference).");

            var (_, clearingErrors) = await _Validator.ValidateAccountAsync(clearingAccountId, cancellationToken);
            if (clearingErrors.Count > 0)
                return new Result(HttpStatusCode.BadRequest, clearingErrors);

            var debit = request.CustomerIsDebit ? request.Amount : 0;
            var credit = request.CustomerIsDebit ? 0 : request.Amount;

            var glResult = await _Sender.Send(new SetOpeningBalanceLineCommand(
                request.FiscalYearId, account!.Id, debit, credit, $"Opening balance — {dealer!.Name}", clearingAccountId, request.CreateUserId),
                cancellationToken);
            if (glResult.StatusCode != HttpStatusCode.OK)
                return glResult;

            // AR open-item side of the opening balance — see
            // docs/architecture/receivables-ddd-migration.md §9/§12 (Phase 5). A credit opening
            // balance (CustomerIsDebit == false) is an on-account credit, not an AR obligation, so no
            // Receivable is created for it — the "unapplied customer credit" concept itself is out of
            // scope (see brief §16/§20, deferred). Idempotent on (OpeningBalance, FiscalYearId,
            // CustomerId): re-submitting the same opening balance (e.g. a correction before the
            // journal is posted) does not create a second AR open item — see the migration doc for why
            // this pass does not attempt to amend an already-created opening-balance Receivable.
            if (request.CustomerIsDebit
                && !await _ReceivableRepository.ExistsForSourceDocumentAsync(SourceDocumentType.OpeningBalance, request.FiscalYearId, request.DealerId, cancellationToken))
            {
                var currency = (await _Sender.Send(new GetDefaultCurrencyQuery(), cancellationToken)).Response;
                var openingDate = DateTime.Now;

                var receivable = Receivable.Create(
                    customerId: request.DealerId,
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

                await _ReceivableRepository.AddAsync(receivable, cancellationToken);
                await _UnitOfWork.SaveChangeAsync(cancellationToken);
            }

            return glResult;
        }

        private static Result BadRequest(string message) => new(HttpStatusCode.BadRequest, [new Error(message)]);
    }
}
