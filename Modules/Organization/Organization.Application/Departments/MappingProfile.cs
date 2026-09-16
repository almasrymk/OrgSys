namespace Organization.Application;

using Organization.Application.Departments.Commands;
using AutoMapper;

public partial class MappingProfile : Profile
{
    public void DepartmentMappingProfile()
    {
        CreateMap<Organization.Domain.Department, DepartmentDto>();
        CreateMap<DepartmentDto, Organization.Domain.Department>();
        CreateMap<Organization.Domain.Department, CreateDepartmentCommand>();
        CreateMap<CreateDepartmentCommand, Organization.Domain.Department>();
        CreateMap<Organization.Domain.Department, UpdateDepartmentCommand>();
        CreateMap<UpdateDepartmentCommand, Organization.Domain.Department>();
        CreateMap<DepartmentDto, CreateDepartmentCommand>();
        CreateMap<CreateDepartmentCommand, DepartmentDto>();
        CreateMap<DepartmentDto, UpdateDepartmentCommand>();
        CreateMap<UpdateDepartmentCommand, DepartmentDto>();
    }
}
