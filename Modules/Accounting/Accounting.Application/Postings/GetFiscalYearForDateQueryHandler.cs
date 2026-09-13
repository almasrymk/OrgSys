namespace Accounting.Application.Postings;

using Accounting.Contracts.Postings;
using OrgSys.SharedKernel;
using System.Net;

/// <summary>Handles the Contracts-facing GetFiscalYearForDateQuery — see that contract's own doc comment.</summary>
public sealed class GetFiscalYearForDateQueryHandler(IRepository<Accounting.Domain.FiscalYear> fiscalYearRepository)
    : IQueryHandler<GetFiscalYearForDateQuery, FiscalYearRangeDto?>
{
    public async Task<Result<FiscalYearRangeDto?>> Handle(GetFiscalYearForDateQuery request, CancellationToken cancellationToken)
    {
        var date = request.Date.Date;
        var fiscalYears = await fiscalYearRepository.GetListByFilterAsync(
            e => e.StartDate.Date <= date && e.EndDate.Date >= date &&
                 e.Status != Status.Deleted && e.Hide != true);
        var fiscalYear = fiscalYears?.FirstOrDefault();

        return new Result<FiscalYearRangeDto?>(
            HttpStatusCode.OK,
            fiscalYear is null ? null : new FiscalYearRangeDto(fiscalYear.Id, fiscalYear.StartDate, fiscalYear.EndDate),
            null);
    }
}
