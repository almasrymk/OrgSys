namespace Receivables.Application.Balances.Queries
{
    using Accounting.Contracts.Accounts;
    using Accounting.Contracts.Postings;
    using MediatR;
    using OrgSys.SharedKernel;
    using Receivables.Contracts.Balances;
    using System.Net;

    /// <summary>
    /// Handles the Contracts-facing GetCustomerAgingQuery. There is no per-invoice OpenItem ledger
    /// in this system (see docs/masterdata-decomposition.md / shared-business-capabilities-review.md
    /// discussion) — aging is instead derived live from the same Journal/JournalItem history
    /// GetCustomerBalanceQuery reads, by FIFO-matching each debit (charge) against later credits
    /// (receipts) posted to the customer's receivable account. Whatever debit amount is still
    /// unmatched as of AsOfDate is aged by the original charge's date. A leftover, unmatched credit
    /// (the customer paid more than they owed) is reported as a negative amount in the Current
    /// bucket — i.e. a credit balance.
    /// </summary>
    public sealed class GetCustomerAgingQueryHandler(
        IReceivableAccountValidator _Validator,
        ISender sender) : IQueryHandler<GetCustomerAgingQuery, AgingBucketDto>
    {
        public async Task<Result<AgingBucketDto>> Handle(GetCustomerAgingQuery request, CancellationToken cancellationToken)
        {
            var (dealer, _, errors) = await _Validator.ValidateCustomerAsync(request.DealerId, cancellationToken);
            if (errors.Count > 0 || dealer?.AccountId is not > 0)
                return new Result<AgingBucketDto>(HttpStatusCode.BadRequest, new AgingBucketDto(0, 0, 0, 0), errors.Count > 0 ? errors : [new Error("Customer does not have a linked receivable account.")]);

            var asOfDate = (request.AsOfDate ?? DateTime.Now).Date;

            var items = (await sender.Send(new GetAccountActivityQuery(dealer.AccountId.Value, asOfDate), cancellationToken)).Response ?? [];

            // FIFO-match each debit "lot" (a charge) against later credits (receipts). Whatever
            // remains open in each lot at the end is what's actually aged; unmatched credit beyond
            // all open lots is a credit balance (reported as negative Current below).
            var openLots = new LinkedList<(DateTime Date, decimal Remaining)>();
            var creditBalance = 0m;

            foreach (var item in items)
            {
                if (item.Debit > 0)
                    openLots.AddLast((item.Date.Date, item.Debit));

                var credit = item.Credit;
                while (credit > 0)
                {
                    if (openLots.First is null)
                    {
                        creditBalance += credit;
                        break;
                    }

                    var lot = openLots.First.Value;
                    var consumed = Math.Min(credit, lot.Remaining);
                    credit -= consumed;
                    var remaining = lot.Remaining - consumed;

                    openLots.RemoveFirst();
                    if (remaining > 0)
                        openLots.AddFirst((lot.Date, remaining));
                }

                if (creditBalance > 0 && openLots.First is not null)
                {
                    // A later charge settles part or all of an existing credit balance first.
                    var lot = openLots.First.Value;
                    var applied = Math.Min(creditBalance, lot.Remaining);
                    creditBalance -= applied;
                    var remaining = lot.Remaining - applied;
                    openLots.RemoveFirst();
                    if (remaining > 0)
                        openLots.AddFirst((lot.Date, remaining));
                }
            }

            decimal current = -creditBalance, days31To60 = 0, days61To90 = 0, over90 = 0;
            foreach (var (date, remaining) in openLots)
            {
                var ageDays = (asOfDate - date).Days;
                if (ageDays <= 30) current += remaining;
                else if (ageDays <= 60) days31To60 += remaining;
                else if (ageDays <= 90) days61To90 += remaining;
                else over90 += remaining;
            }

            return new Result<AgingBucketDto>(HttpStatusCode.OK, new AgingBucketDto(current, days31To60, days61To90, over90), null);
        }
    }
}
