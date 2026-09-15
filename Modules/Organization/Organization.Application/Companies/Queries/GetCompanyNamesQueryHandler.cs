namespace Organization.Application.Companies.Queries
{
    using OrgSys.SharedKernel;
    using Organization.Contracts.Companies;
    using System.Net;

    /// <summary>Handles the Contracts-facing GetCompanyNamesQuery — batch name lookup used by other
    /// modules instead of an EF reference across the module boundary. Mirrors
    /// MasterData.Application.Currencies.Queries.GetCurrencyNamesQueryHandler.</summary>
    public sealed class GetCompanyNamesQueryHandler(IRepository<Organization.Domain.Company> _Repository)
        : IQueryHandler<GetCompanyNamesQuery, Dictionary<long, string?>>
    {
        public async Task<Result<Dictionary<long, string?>>> Handle(GetCompanyNamesQuery request, CancellationToken cancellationToken)
        {
            if (request.CompanyIds.Count == 0)
                return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, [], null);

            var ids = request.CompanyIds.Distinct().ToList();
            var companies = await _Repository.GetListByFilterAsync(e => ids.Contains(e.Id));
            var names = (companies ?? []).ToDictionary(e => e.Id, e => (string?)e.LegalName);

            return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, names, null);
        }
    }
}
