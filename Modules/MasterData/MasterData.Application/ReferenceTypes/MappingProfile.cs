namespace MasterData.Application;

using AutoMapper;
using MasterData.Application.ReferenceTypes.Commands;

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
