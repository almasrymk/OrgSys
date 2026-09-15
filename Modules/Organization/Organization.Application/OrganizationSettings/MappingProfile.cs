namespace Organization.Application;

using AutoMapper;

public partial class MappingProfile : Profile
{
    public void OrganizationSettingsMappingProfile()
    {
        #region OrganizationSettings
        CreateMap<Organization.Domain.OrganizationSettings, OrganizationSettingsDto>();
        CreateMap<OrganizationSettingsDto, Organization.Domain.OrganizationSettings>();
        #endregion
    }
}
