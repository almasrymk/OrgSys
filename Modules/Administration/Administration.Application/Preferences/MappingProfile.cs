namespace Administration.Application;

using Administration.Application.Preferences.Commands;
using Administration.Application.Preferences.Queries;
using AutoMapper;
using Administration.Domain;

public partial class MappingProfile : Profile
{
    public void PreferenceMappingProfile()
    {
        #region Preference
        CreateMap<Preference, PreferenceDto>();
        CreateMap<PreferenceDto, Preference>();
        CreateMap<PreferenceDto, UpdatePreferenceCommand>();
        #endregion
    }
}