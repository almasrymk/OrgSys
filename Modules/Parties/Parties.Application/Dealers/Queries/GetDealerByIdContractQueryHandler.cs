namespace Parties.Application.Dealers.Queries
{
    using OrgSys.SharedKernel;
    using Parties.Contracts.Dealers;
    using System.Net;

    /// <summary>
    /// Handles the Contracts-facing GetDealerByIdQuery — used by other modules (e.g. Accounting's
    /// AR/AP account validators) instead of an IRepository&lt;Dealer&gt; injection across the
    /// module boundary. See docs/dependency-rules.md.
    /// </summary>
    public sealed class GetDealerByIdContractQueryHandler(IRepository<Parties.Domain.Dealer> _Repository)
        : IQueryHandler<GetDealerByIdQuery, DealerLookupDto?>
    {
        public async Task<Result<DealerLookupDto?>> Handle(GetDealerByIdQuery request, CancellationToken cancellationToken)
        {
            var dealer = await _Repository.GetByFilterAsync(e => e.Id == request.Id, string.Empty);
            if (dealer is null)
                return new Result<DealerLookupDto?>(HttpStatusCode.OK, null, null);

            var dto = new DealerLookupDto(dealer.Id, dealer.Name, dealer.TypeId, dealer.AccountId, dealer.Hide, dealer.Status == Status.Deleted);
            return new Result<DealerLookupDto?>(HttpStatusCode.OK, dto, null);
        }
    }
}
