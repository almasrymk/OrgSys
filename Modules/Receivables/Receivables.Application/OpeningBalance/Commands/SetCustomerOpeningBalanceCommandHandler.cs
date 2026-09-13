namespace Receivables.Application.OpeningBalance.Commands
{
    using Accounting.Contracts.Accounts;
    using Accounting.Contracts.Postings;
    using MediatR;
    using OrgSys.SharedKernel;
    using Parties.Contracts.Dealers;
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
        IRepository<Preference> _PreferenceRepository,
        ISender _Sender,
        IReceivableAccountValidator _Validator) : ICommandHandler<SetCustomerOpeningBalanceCommand>
    {
        public async Task<Result> Handle(SetCustomerOpeningBalanceCommand request, CancellationToken cancellationToken)
        {
            if (request.Amount <= 0)
                return BadRequest("Amount must be greater than zero.");

            var (dealer, account, customerErrors) = await _Validator.ValidateCustomerAsync(request.DealerId, cancellationToken);
            if (customerErrors.Count > 0)
                return new Result(HttpStatusCode.BadRequest, customerErrors);

            var preferences = (await _PreferenceRepository.GetListByFilterAsync(
                e => e.Reference == "Dealer" && e.TypeId == (long)DealerType.Client))?.ToList() ?? [];
            var clearingAccountId = long.TryParse(preferences.FirstOrDefault(e => e.Key == "OpeningBalanceClearingAccountId")?.Value, out var cid) ? cid : 0;
            if (clearingAccountId <= 0)
                return BadRequest("Opening balance clearing account is not configured (OpeningBalanceClearingAccountId preference).");

            var (_, clearingErrors) = await _Validator.ValidateAccountAsync(clearingAccountId, cancellationToken);
            if (clearingErrors.Count > 0)
                return new Result(HttpStatusCode.BadRequest, clearingErrors);

            var debit = request.CustomerIsDebit ? request.Amount : 0;
            var credit = request.CustomerIsDebit ? 0 : request.Amount;

            return await _Sender.Send(new SetOpeningBalanceLineCommand(
                request.FiscalYearId, account!.Id, debit, credit, $"Opening balance — {dealer!.Name}", clearingAccountId, request.CreateUserId),
                cancellationToken);
        }

        private static Result BadRequest(string message) => new(HttpStatusCode.BadRequest, [new Error(message)]);
    }
}
