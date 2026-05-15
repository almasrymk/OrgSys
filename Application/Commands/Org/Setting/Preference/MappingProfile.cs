using Application.Commands.Org.Setting.Preference.Commands;
using Application.Commands.Org.Setting.Preference.Queries;
using AutoMapper;
using Entity.Model;
using Entity.ModelView;

public partial class MappingProfile : Profile
{
    public void PreferenceMappingProfile()
    {
        #region Preference
        CreateMap<Preference, PreferenceModelView>();
        CreateMap<PreferenceModelView, Preference>();
        CreateMap<PreferenceModelView, UpdatePreferenceCommand>();
        #endregion
    }
}