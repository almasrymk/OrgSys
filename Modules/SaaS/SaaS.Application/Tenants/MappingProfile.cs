namespace SaaS.Application;

using SaaS.Application.Tenants.Commands;
using AutoMapper;

public partial class MappingProfile
{
    public void TenantMappingProfile()
    {
        #region Tenant
        CreateMap<Tenant, TenantDto>();
        CreateMap<TenantDto, Tenant>();

        CreateMap<Tenant, CreateTenantCommand>();
        CreateMap<CreateTenantCommand, Tenant>();
        CreateMap<Tenant, UpdateTenantCommand>();
        CreateMap<UpdateTenantCommand, Tenant>();

        CreateMap<TenantDto, CreateTenantCommand>();
        CreateMap<CreateTenantCommand, TenantDto>();
        CreateMap<TenantDto, UpdateTenantCommand>();
        CreateMap<UpdateTenantCommand, TenantDto>();
        #endregion
    }
}
