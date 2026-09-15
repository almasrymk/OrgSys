namespace Organization.Application;

using AutoMapper;

public partial class MappingProfile : Profile
{
    public MappingProfile()
    {
        BranchMappingProfile();
        ShiftMappingProfile();
        TableMappingProfile();
        CompanyMappingProfile();
        OrganizationSettingsMappingProfile();
    }
}
