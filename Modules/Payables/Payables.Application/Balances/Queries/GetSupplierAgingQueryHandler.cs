namespace Payables.Application.Balances.Queries
{
    using OrgSys.SharedKernel;
    using Accounting.Application;
    using Payables.Contracts.Balances;
    using System.Net;

    /// <summary>
    /// Handles the Contracts-facing GetSupplierAgingQuery. Mirrors
    /// Receivables.Application.Balances.Queries.GetCustomerAgingQueryHandler for the AP side, with
    /// Debit/Credit swapped: a payable account's "charge" (what we owe more of) posts as Credit,
    /// its "payment" (what reduces the balance) posts as Debit — the opposite of a receivable
    /// account. Aging is derived live from Journal/JournalItem history the same way, by FIFO-
    /// matching each charge (Credit) against later payments (Debit).
    /// </summary>
    public sealed class GetSupplierAgingQueryHandler(
        IPayableAccountValidator _Validator,
        IRepository<JournalItem> _JournalItemRepository) : IQueryHandler<GetSupplierAgingQuery, AgingBucketDto>
    {
        public async Task<Result<AgingBucketDto>> Handle(GetSupplierAgingQuery request, CancellationToken cancellationToken)
        {
            var (dealer, _, errors) = await _Validator.ValidateSupplierAsync(request.DealerId, cancellationToken);
            if (errors.Count > 0 || dealer?.AccountId is not > 0)
                return new Result<AgingBucketDto>(HttpStatusCode.BadRequest, new AgingBucketDto(0, 0, 0, 0), errors.Count > 0 ? errors : [new Error("Supplier does not have a linked payable account.")]);

            var accountId = dealer.AccountId.Value;
            var asOfDate = (request.AsOfDate ?? DateTime.Now).Date;

            var items = (await _JournalItemRepository.GetListByFilterAsync(e =>
                e.AccountId == accountId &&
                e.Journal!.Status != Status.Deleted &&
                e.Journal!.Status != Status.Cancel &&
                (e.Journal!.Posted || (e.Journal!.RefranceTable != null && e.Journal!.RefranceTable != "")) &&
                e.Journal!.Date.Date <= asOfDate,
                "Journal"))?
                .OrderBy(e => e.Journal!.Date)
                .ThenBy(e => e.Id)
                .ToList() ?? [];

            // FIFO-match each charge "lot" (Credit — an amount we now owe) against later payments
            // (Debit). Whatever remains open in each lot at the end is what's actually aged;
            // unmatched payment beyond all open lots is a debit balance we're owed back (reported
            // as negative Current below).
            var openLots = new LinkedList<(DateTime Date, decimal Remaining)>();
            var debitBalance = 0m;

            foreach (var item in items)
            {
                if (item.Credit > 0)
                    openLots.AddLast((item.Journal!.Date.Date, item.Credit));

                var debit = item.Debit;
                while (debit > 0)
                {
                    if (openLots.First is null)
                    {
                        debitBalance += debit;
                        break;
                    }

                    var lot = openLots.First.Value;
                    var consumed = Math.Min(debit, lot.Remaining);
                    debit -= consumed;
                    var remaining = lot.Remaining - consumed;

                    openLots.RemoveFirst();
                    if (remaining > 0)
                        openLots.AddFirst((lot.Date, remaining));
                }

                if (debitBalance > 0 && openLots.First is not null)
                {
                    // A later charge settles part or all of an existing debit balance first.
                    var lot = openLots.First.Value;
                    var applied = Math.Min(debitBalance, lot.Remaining);
                    debitBalance -= applied;
                    var remaining = lot.Remaining - applied;
                    openLots.RemoveFirst();
                    if (remaining > 0)
                        openLots.AddFirst((lot.Date, remaining));
                }
            }

            decimal current = -debitBalance, days31To60 = 0, days61To90 = 0, over90 = 0;
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
