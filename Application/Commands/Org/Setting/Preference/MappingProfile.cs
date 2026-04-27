using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Setting.Preference.Commands;

public partial class MappingProfile : Profile
{
    public void PreferenceMappingProfile()
    {
        #region Preference
        CreateMap<Preference, PreferenceModelView>();
        CreateMap<PreferenceModelView, Preference>();

        CreateMap<Preference, CreatePreferenceCommand>();
        CreateMap<CreatePreferenceCommand, Preference>();
        CreateMap<Preference, UpdatePreferenceCommand>();
        CreateMap<UpdatePreferenceCommand, Preference>();
        CreateMap<Preference, DeletePreferenceCommand>();
        CreateMap<DeletePreferenceCommand, Preference>();

        CreateMap<PreferenceModelView, CreatePreferenceCommand>();
        CreateMap<CreatePreferenceCommand, PreferenceModelView>();
        CreateMap<PreferenceModelView, UpdatePreferenceCommand>();
        CreateMap<UpdatePreferenceCommand, PreferenceModelView>();   
        #endregion
    }
}