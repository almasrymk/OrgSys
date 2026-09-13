namespace Accounting.Application.Postings;

using Accounting.Contracts.Postings;
using OrgSys.SharedKernel;
using System.Net;

/// <summary>
/// Read-only Journal/JournalItem activity for one Account — backs Receivables/Payables balance
/// and aging reports (see the Accounting DDD cleanup report). Same "valid ledger line" filter
/// Post/Reverse/Cancel enforce: the owning Journal must not be Deleted/Cancelled, and must be
/// either Posted directly or owned by another module's resource (RefranceTable set — those are
/// controlled by their own module's lifecycle, not the direct Post/Cancel commands, but still
/// represent real ledger activity once created).
/// </summary>
public sealed class GetAccountActivityQueryHandler(IRepository<Accounting.Domain.JournalItem> journalItemRepository)
    : IQueryHandler<GetAccountActivityQuery, List<AccountActivityLineDto>>
{
    public async Task<Result<List<AccountActivityLineDto>>> Handle(GetAccountActivityQuery request, CancellationToken cancellationToken)
    {
        var asOfDate = request.AsOfDate?.Date;

        var items = (await journalItemRepository.GetListByFilterAsync(e =>
            e.AccountId == request.AccountId &&
            e.Journal!.Status != Status.Deleted &&
            e.Journal!.Status != Status.Cancel &&
            (e.Journal!.Posted || (e.Journal!.RefranceTable != null && e.Journal!.RefranceTable != "")) &&
            (asOfDate == null || e.Journal!.Date.Date <= asOfDate),
            "Journal"))?
            .OrderBy(e => e.Journal!.Date)
            .ThenBy(e => e.Id)
            .Select(e => new AccountActivityLineDto(e.Id, e.Journal!.Date, e.Debit, e.Credit))
            .ToList() ?? [];

        return new Result<List<AccountActivityLineDto>>(HttpStatusCode.OK, items, null);
    }
}
