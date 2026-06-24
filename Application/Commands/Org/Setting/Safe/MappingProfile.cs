using Application.Commands.Org.Setting.Safe.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{
    public void SafeMappingProfile()
    {
        #region Safe
        CreateMap<Safe, SafeDto>()
        .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account.Name));

        CreateMap<SafeDto, Safe>();

        CreateMap<Safe, CreateSafeCommand>();
        CreateMap<CreateSafeCommand, Safe>();
        CreateMap<Safe, UpdateSafeCommand>();
        CreateMap<UpdateSafeCommand, Safe>();
        CreateMap<Safe, DeleteSafeCommand>();
        CreateMap<DeleteSafeCommand, Safe>();

        CreateMap<SafeDto, CreateSafeCommand>();
        CreateMap<CreateSafeCommand, SafeDto>();
        CreateMap<SafeDto, UpdateSafeCommand>();
        CreateMap<UpdateSafeCommand, SafeDto>();
        #endregion
    }
}