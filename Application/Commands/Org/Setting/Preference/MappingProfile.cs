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
        #endregion
    }
}