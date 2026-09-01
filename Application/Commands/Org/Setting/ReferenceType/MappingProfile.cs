using AutoMapper;
using Domain.Entities;
using Application.DTOs;
using Application.Commands.Org.Setting.ReferenceType.Commands;

public partial class MappingProfile : Profile
{
    public void ReferenceTypeMappingProfile()
    {
        #region ReferenceType
        CreateMap<ReferenceType, ReferenceTypeDto>();
        CreateMap<ReferenceTypeDto, ReferenceType>();
        CreateMap<CreateReferenceTypeCommand, ReferenceType>();
        CreateMap<UpdateReferenceTypeCommand, ReferenceType>();
        #endregion
    }
}
