namespace Parties.Application.Dealers.Queries
{
    using Accounting.Contracts.Postings;
    using MediatR;
    using System.Net;

    /// <summary>Customer Balance = Sum(Debit - Credit) of the ledger activity posted against the
    /// customer's receivable account — resolved through Accounting.Contracts.Postings.GetAccountActivityQuery
    /// (see that contract for the exact "valid ledger line" filter) instead of an
    /// IRepository&lt;JournalItem&gt; reference across the module boundary.</summary>
    public sealed record GetDealerBalanceQuery(long DealerId, DateTime? AsOfDate) : IRequest<Result<decimal>>;

    public sealed class GetDealerBalanceQueryHandler(
        IRepository<Parties.Domain.Dealer> _DealerRepository,
        ISender sender) : IRequestHandler<GetDealerBalanceQuery, Result<decimal>>
    {
        public async Task<Result<decimal>> Handle(GetDealerBalanceQuery request, CancellationToken cancellationToken)
        {
            var dealer = await _DealerRepository.GetByFilterAsync(e => e.Id == request.DealerId, string.Empty);
            if (dealer is null)
                return new Result<decimal>(HttpStatusCode.NotFound, 0, [new Error("Customer not found.")]);

            if (dealer.AccountId is not > 0)
                return new Result<decimal>(HttpStatusCode.BadRequest, 0, [new Error($"Customer '{dealer.Name}' does not have a linked receivable account.")]);

            var items = (await sender.Send(new GetAccountActivityQuery(dealer.AccountId.Value, request.AsOfDate), cancellationToken)).Response ?? [];

            var balance = items.Sum(e => e.Debit - e.Credit);
            return new Result<decimal>(HttpStatusCode.OK, balance, null);
        }
    }
}
