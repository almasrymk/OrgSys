using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Country.Commands;

public partial class MappingProfile : Profile
{
    public void CountryMappingProfile()
    {
        #region Country
        CreateMap<Country, CountryModelView>();
        CreateMap<CountryModelView, Country>();
        CreateMap<Country, CreateCountryCommand>();
        CreateMap<CreateCountryCommand, Country>();
        CreateMap<Country, UpdateCountryCommand>();
        CreateMap<UpdateCountryCommand, Country>();
        CreateMap<Country, DeleteCountryCommand>();
        CreateMap<DeleteCountryCommand, Country>();
        #endregion
    }
}