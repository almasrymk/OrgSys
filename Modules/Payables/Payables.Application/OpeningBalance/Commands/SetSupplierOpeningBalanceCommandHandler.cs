namespace Payables.Application.OpeningBalance.Commands
{
    using Accounting.Contracts.Accounts;
    using Accounting.Contracts.Postings;
    using MediatR;
    using OrgSys.SharedKernel;
    using Parties.Contracts.Dealers;
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
        IRepository<Preference> _PreferenceRepository,
        ISender _Sender,
        IPayableAccountValidator _Validator,
        IReceivableAccountValidator _AccountValidator) : ICommandHandler<SetSupplierOpeningBalanceCommand>
    {
        public async Task<Result> Handle(SetSupplierOpeningBalanceCommand request, CancellationToken cancellationToken)
        {
            if (request.Amount <= 0)
                return BadRequest("Amount must be greater than zero.");

            var (dealer, account, supplierErrors) = await _Validator.ValidateSupplierAsync(request.DealerId, cancellationToken);
            if (supplierErrors.Count > 0)
                return new Result(HttpStatusCode.BadRequest, supplierErrors);

            var preferences = (await _PreferenceRepository.GetListByFilterAsync(
                e => e.Reference == "Dealer" && e.TypeId == (long)DealerType.Supplier))?.ToList() ?? [];
            var clearingAccountId = long.TryParse(preferences.FirstOrDefault(e => e.Key == "OpeningBalanceClearingAccountId")?.Value, out var cid) ? cid : 0;
            if (clearingAccountId <= 0)
                return BadRequest("Opening balance clearing account is not configured (OpeningBalanceClearingAccountId preference).");

            var (_, clearingErrors) = await _AccountValidator.ValidateAccountAsync(clearingAccountId, cancellationToken);
            if (clearingErrors.Count > 0)
                return new Result(HttpStatusCode.BadRequest, clearingErrors);

            var debit = request.SupplierIsDebit ? request.Amount : 0;
            var credit = request.SupplierIsDebit ? 0 : request.Amount;

            return await _Sender.Send(new SetOpeningBalanceLineCommand(
                request.FiscalYearId, account!.Id, debit, credit, $"Opening balance — {dealer!.Name}", clearingAccountId, request.CreateUserId),
                cancellationToken);
        }

        private static Result BadRequest(string message) => new(HttpStatusCode.BadRequest, [new Error(message)]);
    }
}
