namespace MasterData.Application.Currencies.Queries
{
    using OrgSys.SharedKernel;
    using MasterData.Contracts.Currencies;
    using System.Net;

    /// <summary>
    /// Handles the Contracts-facing GetDefaultCurrencyQuery — used by other modules (e.g.
    /// Receivables/Payables opening balance postings) instead of an IRepository&lt;Currency&gt;
    /// injection across the module boundary. See docs/dependency-rules.md.
    /// </summary>
    public sealed class GetDefaultCurrencyQueryHandler(IRepository<Currency> _Repository)
        : IQueryHandler<GetDefaultCurrencyQuery, CurrencyLookupDto?>
    {
        public async Task<Result<CurrencyLookupDto?>> Handle(GetDefaultCurrencyQuery request, CancellationToken cancellationToken)
        {
            var currency = await _Repository.GetByFilterAsync(e => e.IsDefault, string.Empty);
            if (currency is null)
                return new Result<CurrencyLookupDto?>(HttpStatusCode.OK, null, null);

            var dto = new CurrencyLookupDto(currency.Id, currency.Name, currency.Rate, currency.IsDefault);
            return new Result<CurrencyLookupDto?>(HttpStatusCode.OK, dto, null);
        }
    }
}
