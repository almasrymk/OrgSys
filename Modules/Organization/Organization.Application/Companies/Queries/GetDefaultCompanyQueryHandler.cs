namespace Organization.Application.Companies.Queries
{
    using OrgSys.SharedKernel;
    using Organization.Contracts.Companies;
    using System.Net;

    /// <summary>Handles the Contracts-facing GetDefaultCompanyQuery — resolves "the" Company for
    /// modules that need one but OrgSys has no multi-company UI to pick from yet. Mirrors
    /// MasterData.Application.Currencies.Queries.GetDefaultCurrencyQueryHandler.</summary>
    public sealed class GetDefaultCompanyQueryHandler(IRepository<Organization.Domain.Company> _Repository)
        : IQueryHandler<GetDefaultCompanyQuery, CompanyLookupDto?>
    {
        public async Task<Result<CompanyLookupDto?>> Handle(GetDefaultCompanyQuery request, CancellationToken cancellationToken)
        {
            var company = await _Repository.GetByFilterAsync(e => e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true, string.Empty);
            if (company is null || company.Id == 0)
                return new Result<CompanyLookupDto?>(HttpStatusCode.OK, null, null);

            var dto = new CompanyLookupDto(company.Id, company.LegalName, company.TradeName, !company.Hide);
            return new Result<CompanyLookupDto?>(HttpStatusCode.OK, dto, null);
        }
    }
}
