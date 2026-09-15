namespace Organization.Application.OrganizationSettings.Commands
{
    using OrgSys.SharedKernel;
    using System.Net;

    /// <summary>
    /// Explicit business action (brief's CQRS RULE — not generic Create&lt;T&gt;/Update&lt;T&gt;):
    /// OrganizationSettings is a singleton-per-Company value, so "update" means "create the row on
    /// first use, otherwise update the existing one" rather than requiring a separate Create step the
    /// caller has to orchestrate. Deliberately does not inherit UpdateCommandHandler&lt;,&gt; (that
    /// base requires the row to already exist).
    /// </summary>
    public sealed record UpdateOrganizationSettingsCommand(
        long CompanyId,
        long? DefaultCurrencyId,
        long? DefaultCountryId,
        string? DefaultTimeZone,
        int? FiscalYearStartMonth,
        int? FiscalYearStartDay) : ICommand;

    public sealed class UpdateOrganizationSettingsCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<Organization.Domain.OrganizationSettings> _Repository) : ICommandHandler<UpdateOrganizationSettingsCommand>
    {
        public async Task<Result> Handle(UpdateOrganizationSettingsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existing = await _Repository.GetByFilterAsync(e => e.CompanyId == request.CompanyId, string.Empty);

                if (existing != null && existing.Id > 0)
                {
                    existing.DefaultCurrencyId = request.DefaultCurrencyId;
                    existing.DefaultCountryId = request.DefaultCountryId;
                    existing.DefaultTimeZone = request.DefaultTimeZone;
                    existing.FiscalYearStartMonth = request.FiscalYearStartMonth;
                    existing.FiscalYearStartDay = request.FiscalYearStartDay;

                    await _Repository.UpdateAsync(existing);
                }
                else
                {
                    await _Repository.CreateAsync(new Organization.Domain.OrganizationSettings
                    {
                        CompanyId = request.CompanyId,
                        DefaultCurrencyId = request.DefaultCurrencyId,
                        DefaultCountryId = request.DefaultCountryId,
                        DefaultTimeZone = request.DefaultTimeZone,
                        FiscalYearStartMonth = request.FiscalYearStartMonth,
                        FiscalYearStartDay = request.FiscalYearStartDay
                    });
                }

                if (await _UnitOfWork.SaveChangeAsync(cancellationToken) > 0)
                    return new Result(HttpStatusCode.OK, null);

                return new Result(HttpStatusCode.InternalServerError, new List<Error> { new Error("Error") });
            }
            catch (Exception ex)
            {
                return new Result(HttpStatusCode.InternalServerError, new List<Error> { new Error(ex.Message) });
            }
        }
    }
}
