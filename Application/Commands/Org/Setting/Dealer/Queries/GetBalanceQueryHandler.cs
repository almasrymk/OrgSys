namespace Application.Commands.Org.Setting.Dealer.Queries
{
    using Domain.Abstraction;
    using Domain.Entities;
    using Domain.Enums;
    using Domain.Shared;
    using MediatR;
    using System.Net;

    /// <summary>Customer Balance = Sum(Debit - Credit) of the JournalItems posted against the
    /// customer's receivable account. A journal is treated as valid ledger history when its Status
    /// isn't Deleted/Cancel AND it is either <c>Posted</c> (the manual Journal Draft→Post workflow)
    /// OR carries a <c>RefranceTable</c> (an Invoice/Financial-sourced journal — these integrations
    /// never set <c>Posted</c>, they're final at creation and governed by Status instead; confirmed by
    /// reading InvoiceJournalIntegration.cs and TransactionJournalIntegration.cs). A literal
    /// <c>Posted == true</c> filter would silently zero out every invoice-driven balance.</summary>
    public sealed record GetDealerBalanceQuery(long DealerId, DateTime? AsOfDate) : IRequest<Result<decimal>>;

    public sealed class GetDealerBalanceQueryHandler(
        IRepository<Domain.Entities.Dealer> _DealerRepository,
        IRepository<JournalItem> _JournalItemRepository) : IRequestHandler<GetDealerBalanceQuery, Result<decimal>>
    {
        public async Task<Result<decimal>> Handle(GetDealerBalanceQuery request, CancellationToken cancellationToken)
        {
            var dealer = await _DealerRepository.GetByFilterAsync(e => e.Id == request.DealerId, string.Empty);
            if (dealer is null)
                return new Result<decimal>(HttpStatusCode.NotFound, 0, [new Error("Customer not found.")]);

            if (dealer.AccountId is not > 0)
                return new Result<decimal>(HttpStatusCode.BadRequest, 0, [new Error($"Customer '{dealer.Name}' does not have a linked receivable account.")]);

            var accountId = dealer.AccountId.Value;
            var asOfDate = request.AsOfDate?.Date;

            var items = await _JournalItemRepository.GetListByFilterAsync(e =>
                e.AccountId == accountId &&
                e.Journal!.Status != Status.Deleted &&
                e.Journal!.Status != Status.Cancel &&
                (e.Journal!.Posted || (e.Journal!.RefranceTable != null && e.Journal!.RefranceTable != "")) &&
                (asOfDate == null || e.Journal!.Date.Date <= asOfDate));

            var balance = (items ?? []).Sum(e => e.Debit - e.Credit);
            return new Result<decimal>(HttpStatusCode.OK, balance, null);
        }
    }
}
