namespace Organization.Application.OrganizationSettings.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    /// <summary>Returns the settings row for a Company, or an empty (Id = 0, all-defaults) DTO if the
    /// company hasn't configured any settings yet — GetCommandHandler's base behavior already treats
    /// "not found" this way, which matches "settings are optional until configured" (brief §1.10)
    /// better than a 404. Served through a small custom controller (not BaseController&lt;&gt;), so
    /// this only needs ICommand&lt;TResponse&gt;, not the Activator-driven IGetByIdQuery shape.</summary>
    public sealed record GetOrganizationSettingsQuery(long CompanyId) : ICommand<OrganizationSettingsDto>;

    public sealed class GetByCompanyIdQueryHandler(IRepository<Organization.Domain.OrganizationSettings> _Repository, IMapper mapper) : GetCommandHandler<GetOrganizationSettingsQuery, Organization.Domain.OrganizationSettings, OrganizationSettingsDto>(_Repository, mapper)
    {
        public override Expression<Func<Organization.Domain.OrganizationSettings, bool>> CreateFilter(GetOrganizationSettingsQuery request)
        {
            return e => e.CompanyId == request.CompanyId && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
